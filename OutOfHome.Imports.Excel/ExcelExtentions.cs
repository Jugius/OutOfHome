using OfficeOpenXml;
using OfficeOpenXml.Style;
using OutOfHome.Models.Pois;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace OutOfHome.Imports.Excel
{
    internal static class ExcelExtentions
    {
        public static IEnumerable<ExcelBoard> ConvertSheetToObjects(this ExcelWorksheet worksheet, IEnumerable<BoardPropertySetter> setters)
        {
            var rows = worksheet.Cells
            .Select(cell => cell.Start.Row)
            .Distinct()
            .OrderBy(x => x);

            Dictionary<PropertySetter, int> dic = GetPropertySettersDictionary(setters);            
            var collection = rows.Skip(1).Select(row =>
            {
                var tnew = new ExcelBoard { Address = new OutOfHome.Models.Boards.BoardAddress { City = new OutOfHome.Models.City() } };
                foreach(var setter in setters)
                {
                    var val = worksheet.Cells[row, dic[setter]];
                    setter.SetValue(tnew, val);
                }                
                return tnew;
            });

            //Send it back
            return collection;
        }
        public static IEnumerable<Poi> ConvertSheetToObjects(this ExcelWorksheet worksheet, IEnumerable<PoiPropertySetter> setters)
        {
            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            Dictionary<PropertySetter, int> dic = GetPropertySettersDictionary(setters);
            var collection = rows.Skip(1).Select(row =>
            {
                var tnew = new Poi(null, new OutOfHome.Models.Location(0, 0), "Excel");
                foreach(var setter in setters)
                {
                    var val = worksheet.Cells[row, dic[setter]];
                    setter.SetValue(tnew, val);
                }
                return tnew;
            });

            //Send it back
            return collection;
        }
        public static Dictionary<PropertySetter, int> GetPropertySettersDictionary(IEnumerable<PropertySetter> setters)
        {
            int i = 0;
            Dictionary<PropertySetter, int> dic = new Dictionary<PropertySetter, int>();
            foreach(var s in setters)
            {
                dic.Add(s, ++i);
            }
            return dic;
        }

        public static System.Drawing.Color GetBackgroundColor(this ExcelRange cell)
        {
            if(cell == null)
                return Color.Empty;

            var color = cell.Style.Fill.BackgroundColor;
            if(color == null)
                return Color.Empty;
                        
            return Convert(color);
        }
        private static System.Drawing.Color Convert(ExcelColor color)
        {
            
            string colorcode = color.Rgb.TrimStart('#');
            
            if(colorcode.Length == 6)
                return Color.FromArgb(255, // hardcoded opaque
                            int.Parse(colorcode.Substring(0, 2), NumberStyles.HexNumber),
                            int.Parse(colorcode.Substring(2, 2), NumberStyles.HexNumber),
                            int.Parse(colorcode.Substring(4, 2), NumberStyles.HexNumber));
            else if(colorcode.Length == 8) // assuming length of 8
                return Color.FromArgb(
                            int.Parse(colorcode.Substring(0, 2), NumberStyles.HexNumber),
                            int.Parse(colorcode.Substring(2, 2), NumberStyles.HexNumber),
                            int.Parse(colorcode.Substring(4, 2), NumberStyles.HexNumber),
                            int.Parse(colorcode.Substring(6, 2), NumberStyles.HexNumber));
            else
                throw new Exception("Color not valid");            
        }
        public static Uri GetUri(this ExcelRange cell)
        {
            Uri u;
            if(cell == null)
                return null;

            if(cell.Hyperlink != null)
                return cell.Hyperlink;


            if(cell.Value != null)
            {
                string url = cell.GetValue<string>().Trim();
                if(!string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url, UriKind.Absolute, out u))
                    return u;
            }

            if(cell.Formula != null)
            {
                string formula = cell.Formula;
                if(formula.Contains("HYPERLINK"))
                {
                    int Start, End;
                    Start = formula.IndexOf('"', 0) + 1;
                    End = formula.IndexOf('"', Start);
                    formula = formula.Substring(Start, End - Start);
                    if(Uri.TryCreate(formula, UriKind.Absolute, out u))
                        return u;
                }
            }
            return null;
        }
    }
    
}
