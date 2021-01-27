using OutOfHome.Models;
using OutOfHome.Models.Boards;
using OutOfHome.Models.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OutOfHome.Exports
{
    public class BoardPropertyGetter : PropertyGetter
    {
        private const string GoogleMapLink = @"https://www.google.com/maps/place/";
        private const string GoogleStreetsViewLink = @"https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=";

        private static readonly Regex AspectRegex = new Regex(Regex.Escape("{") + "([^{}]*)" + Regex.Escape("}"));
        private static readonly HashSet<BoardProperty> PropertiesWithHyperlinks = new HashSet<BoardProperty>
        {
            BoardProperty.URL_DoorsMap, BoardProperty.URL_DoorsPhoto, BoardProperty.URL_GoogleMapPoint, BoardProperty.URL_Map, BoardProperty.URL_Photo, BoardProperty.URL_StreetsView
        };
        public BoardProperty Kind { get; set; }
        public string Name
        {
            set { _name = string.IsNullOrEmpty(value) ? null : value; }
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    if (this.Kind == BoardProperty.Custom)
                        return this.AspectName;
                    else
                        return DefaultNames[this.Kind];
                }
                return _name;
            }
        }
        private string _name;
        public string AspectName { get; set; }
        public virtual string NumberFormat => this.Kind switch
        {
            BoardProperty.Price => "### ### ##0",
            BoardProperty.DoorsId => "########0",
            BoardProperty.OTS => "##0",
            BoardProperty.GRP => "##0.00",
            _ => null,
        };
        public bool IsHyperlink { get; set; } = false;
        public BoardPropertyGetter(BoardProperty kind) 
        { 
            this.Kind = kind;
            this.IsHyperlink = PropertiesWithHyperlinks.Contains(kind);
        }        

        public BoardPropertyGetter() { }
        public object GetPropertyValueFrom(Board board) => this.Kind switch
        {
            BoardProperty.ProviderID => board.ProviderID,
            BoardProperty.Provider => board.Provider,
            BoardProperty.Supplier => board.Supplier,
            BoardProperty.SupplierCode => board.SupplierCode,
            BoardProperty.Region => board.Address.City.Region,
            BoardProperty.City => board.Address.City,
            BoardProperty.Address => board.Address.GetFormattedAddress(),
            BoardProperty.Side => board.Side,
            BoardProperty.Kind => board.Type,
            BoardProperty.Size => board.Size,
            BoardProperty.URL_Photo => board.Photo,// ?? board.DoorsInfo?.Photo,
            BoardProperty.URL_Map => board.Map,
            BoardProperty.URL_DoorsPhoto => board.DoorsInfo?.Photo,// ?? board.Photo,
            BoardProperty.URL_DoorsMap => board.DoorsInfo?.Map,
            BoardProperty.URL_GoogleMapPoint => new Uri(GoogleMapLink + board.Location?.ToString()),
            BoardProperty.URL_StreetsView => new Uri(GoogleStreetsViewLink + board.Location?.ToString()),
            BoardProperty.Light => board.Lighting ? "+" : "-",

            BoardProperty.DoorsId => board.DoorsInfo?.DoorsID,
            BoardProperty.OTS => board.DoorsInfo?.OTS,
            BoardProperty.GRP => board.DoorsInfo?.GRP,
            BoardProperty.Location => board.Location?.ToString(),
            BoardProperty.Street => board.Address.Street,
            BoardProperty.StreetNumber => board.Address.StreetNumber,
            BoardProperty.AddressDescription => board.Address.Description,
            BoardProperty.Custom => GetPropertyAspectValueFrom(board),
            BoardProperty.Color => (board as IColored)?.Color,

            BoardProperty.OccSource => (board as IHaveSupplierContent)?.Occupation?.OriginOccupationString,

            _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
        };
        public override object GetPropertyValueFrom(object source)
        {
            if(source == null) throw new ArgumentNullException();
            return GetPropertyValueFrom(source as Board);
        }
        public object GetPropertyValueFrom(BaseBoardModelView board) => this.Kind switch
        {
            BoardProperty.ProviderID => board.ProviderID,
            BoardProperty.Provider => board.Provider,
            BoardProperty.Supplier => board.Supplier,
            BoardProperty.SupplierCode => board.Code,

            BoardProperty.Region => board.Region,
            BoardProperty.City => board.City,
            BoardProperty.Street => board.Street,
            BoardProperty.StreetNumber => board.StreetHouse,
            BoardProperty.AddressDescription => board.AddressDescription,
            BoardProperty.Address => board.GetFormattedAddress(),

            BoardProperty.Side => board.Side,
            BoardProperty.Kind => board.Type,
            BoardProperty.Size => board.Size,

            BoardProperty.URL_Photo => board.Photo,// ?? board.DoorsInfo?.Photo,
            BoardProperty.URL_Map => board.Map,
            BoardProperty.URL_DoorsPhoto => board.PhotoDoors,
            BoardProperty.URL_DoorsMap => board.MapDoors,

            BoardProperty.URL_GoogleMapPoint => new Uri(GoogleMapLink + GetFormattedLocation(board)),
            BoardProperty.URL_StreetsView => new Uri(GoogleStreetsViewLink + GetFormattedLocation(board)),
            
            BoardProperty.Light => board.Lighting ? "+" : "-",

            BoardProperty.DoorsId => NullableOrNull(board.DoorsDix),
            BoardProperty.OTS => NullableOrNull(board.OTS),
            BoardProperty.GRP => NullableOrNull(board.GRP),
            BoardProperty.Location => GetFormattedLocation(board),
            
            BoardProperty.OccSource => throw new NotImplementedException(),
            BoardProperty.Custom => GetPropertyAspectValueFrom(board),

            BoardProperty.Color => (board as IColored)?.Color,

            _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
        };
        public object GetPropertyValueFrom(BaseBoardModelView board, DateTimePeriod period) =>
            this.Kind == BoardProperty.Price ? (board as IHaveSupplierContent)?.Price?.GetValue(period) : GetPropertyValueFrom(board);
        public object GetPropertyValueFrom(Board board, DateTimePeriod period) =>
            this.Kind == BoardProperty.Price ? (board as IHaveSupplierContent)?.Price?.GetValue(period) : GetPropertyValueFrom(board);
        private static string GetFormattedLocation(BaseBoardModelView board) => string.Format(CultureInfo.InvariantCulture, "{0:0.00000000},{1:0.00000000}", board.Latitude, board.Longitude);
        
        private static object NullableOrNull(int? val) => val.HasValue ? val.Value as object : null;
        private static object NullableOrNull(double? val) => val.HasValue ? val.Value as object : null;              
        protected virtual string GetPropertyAspectValueFrom(object item)
        {
            MatchCollection maches = AspectRegex.Matches(this.AspectName);
            string aspect = this.AspectName;
            foreach (Match m in maches)
            {
                var result = item.GetPropertyValueByName(m.Groups[1].Value);
                if (result == null) return aspect;
                aspect = aspect.Replace(m.Value, result.ToString());
            }
            return aspect;
        }
        static readonly Dictionary<BoardProperty, string> DefaultNames = new Dictionary<BoardProperty, string>()
        {
            { BoardProperty.ProviderID, "код" },
            { BoardProperty.Provider, "источник" },
            { BoardProperty.Supplier, "оператор" },
            { BoardProperty.SupplierCode, "код оператора" },
            { BoardProperty.Region, "область" },
            { BoardProperty.City, "город" },
            { BoardProperty.Address, "адрес" },
            { BoardProperty.Kind, "тип" },
            { BoardProperty.Side, "сторона" },
            { BoardProperty.Size, "размер" },
            { BoardProperty.Light, "свет" },
            { BoardProperty.Price, "прайс" },
            { BoardProperty.URL_Map, "схема" },
            { BoardProperty.URL_Photo, "фото" },
            { BoardProperty.URL_DoorsPhoto, "фото Doors"},
            { BoardProperty.URL_DoorsMap, "схема Doors" },
            { BoardProperty.URL_GoogleMapPoint, "карта" },
            { BoardProperty.URL_StreetsView, "панорама"},
            { BoardProperty.DoorsId, "DoorsID" },
            { BoardProperty.OTS, "OTS" },
            { BoardProperty.GRP, "GRP" },
            { BoardProperty.Location, "коорд" },
            { BoardProperty.OccSource, "occup" },
            { BoardProperty.Street, "улица" },
            { BoardProperty.StreetNumber, "дом" },
            { BoardProperty.AddressDescription, "описание"},
            
            { BoardProperty.Custom, "Custom" },
            { BoardProperty.Color, "цвет" }
        };
    }
}
