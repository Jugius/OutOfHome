using OutOfHome.Models;
using OutOfHome.Models.Boards;
using OutOfHome.Models.Views;
using System;
using System.Globalization;

namespace OutOfHome.Exports
{
    public class BoardPropertyGetter : PropertyGetter<Board>
    {
        private const string GoogleMapLink = @"https://www.google.com/maps/place/";
        private const string GoogleStreetsViewLink = @"https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=";
        public BoardProperty Kind { get; set; }
        public BoardPropertyGetter(BoardProperty kind)
        {
            this.Kind = kind;
        }
        public BoardPropertyGetter() { }
        public override object GetPropertyValueFrom(Board board) => this.Kind switch
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

            BoardProperty.DoorsId => NullableOrNull(board.DoorsInfo?.DoorsID),
            BoardProperty.OTS => NullableOrNull(board.DoorsInfo?.OTS),
            BoardProperty.GRP => NullableOrNull(board.DoorsInfo?.GRP),
            BoardProperty.Location => board.Location?.ToString(),
            BoardProperty.Street => board.Address.Street,
            BoardProperty.StreetNumber => board.Address.StreetNumber,
            BoardProperty.AddressDescription => board.Address.Description,
            //BoardProperty.Custom => GetCustomPropertyValueFrom(board),
            //BoardProperty.Color => (board as IColored)?.Color,

            BoardProperty.OccSource => (board as IHaveSupplierContent)?.Occupation?.OriginOccupationString,

            _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
        };
        public override object GetPropertyValueFrom(object source)
        {
            if(source == null) throw new ArgumentNullException(nameof(source));
            return source switch
            {
                BaseBoardModelView b => GetPropertyValueFrom(b),
                Board b => GetPropertyValueFrom(b),
                _ => throw new Exception($"Неизвестный тип {source.GetType()}")
            };
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
            //BoardProperty.Custom => GetCustomPropertyValueFrom(board),
            //BoardProperty.Color => (board as IColored)?.Color,

            _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
        };
        public object GetPropertyValueFrom(BaseBoardModelView board, DateTimePeriod period) =>
            this.Kind == BoardProperty.Price ? (board as IHaveSupplierContent)?.Price?.GetValue(period) : GetPropertyValueFrom(board);
        public object GetPropertyValueFrom(Board board, DateTimePeriod period) =>
            this.Kind == BoardProperty.Price ? (board as IHaveSupplierContent)?.Price?.GetValue(period) : GetPropertyValueFrom(board);
        private static string GetFormattedLocation(BaseBoardModelView board) => string.Format(CultureInfo.InvariantCulture, "{0:0.00000000},{1:0.00000000}", board.Latitude, board.Longitude);
        private static int? NullableOrNull(int? val) => val.HasValue && val.Value != 0 ? val.Value : null;
        private static double? NullableOrNull(double? val) => val.HasValue && val.Value != 0 ? val.Value : null;
    }
}
