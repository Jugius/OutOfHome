using OutOfHome.Models.Views;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OutOfHome.Exports.GoogleMaps
{
    public class KmlWriter
    {
        KmlWriteParameters _parameters;
        public string Write(IList<ColoredBoard> boards, KmlWriteParameters parameters)
        {
            this._parameters = parameters;
            
            if (boards.Count == 0) return null;

            var colors = boards.Select(a => a.Color).Distinct().ToList();
            var styles = CreateStyles(colors).ToList();

            var document = new Document();
            document.Name = System.IO.Path.GetFileNameWithoutExtension(_parameters.FilePath);

            if (_parameters.LayersSelector != null)
            {
                foreach (var group in boards.GroupBy(_parameters.LayersSelector))
                {
                    var marks = group.OrderBy(a => a.City).Select(a => CreatePlacemark(a, styles, _parameters.CreateLayers ? null : group.Key));
                    if (_parameters.CreateLayers)
                    {
                        Folder f = new Folder { Name = group.Key };
                        foreach (var mark in marks)
                        {
                            f.AddFeature(mark);
                        }
                        document.AddFeature(f);
                    }
                    else
                    {
                        foreach (var mark in marks)
                        {
                            document.AddFeature(mark);
                        }
                    }
                }
            }
            else
            {
                var marks = boards.OrderBy(a => a.City).Select(a => CreatePlacemark(a, styles, null));
                foreach (var mark in marks)
                {
                    document.AddFeature(mark);
                }
            }

            foreach (var s in styles)
            {
                document.AddStyle(s);
            }

            KmlFile kml = KmlFile.Create(document, false);
            KmzFile k = KmzFile.Create(kml);

            string path = _parameters.FilePath;
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            using (var stream = System.IO.File.OpenWrite(path))
            {
                k.Save(stream);
            }

            return path;
        }
        private Placemark CreatePlacemark(ColoredBoard b, IEnumerable<Style> styles, string groupName)
        {
            List<string> nameParts = new List<string>(5);
            
            if (!string.IsNullOrEmpty(groupName))
                nameParts.Add(groupName);

            if (_parameters.NameCity && _parameters.GroupingProperty != GroupingProperty.City)
                nameParts.Add(b.City);

            if (_parameters.NameCode && !string.IsNullOrEmpty(b.Code))
                nameParts.Add(b.Code);

            if (_parameters.NameAddress)
                nameParts.Add(b.GetFormattedAddress());

            var placemark = new Placemark();

            var c = b.Color;

            placemark.Name = string.Join(' ', nameParts);
            var styleId = styles.First(a => a.Id == $"colorIcon_{c.R}_{c.G}_{c.B}").Id;
            placemark.StyleUrl = new Uri($"#{styleId}", UriKind.Relative);
            placemark.Geometry = new SharpKml.Dom.Point
            {
                Coordinate = new Vector(b.Latitude, b.Longitude)
            };

            string description = string.Empty;

            if (_parameters.CardType)
                description += $"{b.Type} {b.Size}";

            if (_parameters.CardSide)
            {
                if (string.IsNullOrEmpty(description))
                    description += "Сторона " + b.Side;
                else
                    description += ", сторона " + b.Side;

            }

            if(_parameters.CardMedia && b.OTS.HasValue && b.OTS != 0)
                description += $"<br>OTS: {b.OTS} 000, GRP: {b.GRP}";

            if(_parameters.CardSupplier)
                description += $"<br>{b.Supplier}";

            if (_parameters.CardCode && !string.IsNullOrEmpty(b.Code))
            {
                if (_parameters.CardSupplier)
                    description += $" {b.Code}";
                else
                    description += $"<br>{b.Code}";
            }

            Uri photo = _parameters.PreferredPhoto switch
            {
                KmlWriteParameters.PreferredSource.Doors => b.PhotoDoors ?? b.Photo,
                KmlWriteParameters.PreferredSource.Supplier => b.Photo ?? b.PhotoDoors,
                _ => throw new NotImplementedException(),
            };

            if(photo != null)
                description += $"<br><img src=\"{photo}\" height =\"300px\" width=\"auto\"/>";

            description = $"<![CDATA[{description}]]>";
            placemark.Description = new Description { Text = description };

            return placemark;
        }

        private static IEnumerable<Style> CreateStyles(IEnumerable<Color> colors)
        {
            foreach (var c in colors)
            {
                var style = new Style();
                style.Id = $"colorIcon_{c.R}_{c.G}_{c.B}";
                style.Icon = new IconStyle();
                style.Icon.Color = new SharpKml.Base.Color32(c.A, c.B, c.G, c.R);
                style.Icon.ColorMode = ColorMode.Normal;
                style.Icon.Icon = new IconStyle.IconLink(new Uri("http://www.gstatic.com/mapspro/images/stock/503-wht-blank_maps.png"));
                style.Icon.Scale = 1;
                style.Label = new LabelStyle();
                style.Label.Scale = 0;
                yield return style;
            }
        }

    }
}
