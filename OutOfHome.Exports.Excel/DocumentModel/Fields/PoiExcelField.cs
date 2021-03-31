using OutOfHome.Models.Pois;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModel.Fields
{
    public sealed class PoiExcelField : PoiPropertyGetter, IExcelField
    {
        private static readonly HashSet<PoiProperty> PropertiesContainsHyperlinks = new HashSet<PoiProperty>
        {
             PoiProperty.URL_Map
        };
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _columnWidth = 0;
        public bool IsHyperlink { get; set; }
        public string ColumnHeader
        {
            get => _columnHeader ?? DefaultNames[this.Kind];
            set => _columnHeader = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _columnHeader;
        public string NumberFormat => this.Kind switch
        {
            PoiProperty.Latitude => "0.00000###",
            PoiProperty.Longitude => "0.00000###",
            _ => null,
        };
        public PoiExcelField(PoiProperty kind) : base(kind)
        {
            this.IsHyperlink = PropertiesContainsHyperlinks.Contains(kind);
        }
        public static List<PoiExcelField> GetDefaultColumns() => new List<PoiExcelField>(20)
            {
            //base
                new PoiExcelField(PoiProperty.Location),
                new PoiExcelField(PoiProperty.FormattedAddress),
                new PoiExcelField(PoiProperty.Description),               

            //parsed                
                new PoiExcelField(PoiProperty.Country),
                new PoiExcelField(PoiProperty.Region),
                new PoiExcelField(PoiProperty.City),
                new PoiExcelField(PoiProperty.District),
                new PoiExcelField(PoiProperty.Street),
                new PoiExcelField(PoiProperty.StreetNumber),
                //new PoiExcelField(PoiProperty.Zip),
                new PoiExcelField(PoiProperty.URL_Map),
                new PoiExcelField(PoiProperty.OriginalAddress),
                
                //new BindExcelField(PoiProperty.ProviderPlaceId)
        };
        private static int GetDefaultColumnWidth(PoiProperty kind)
        {
            switch(kind)
            {
                case PoiProperty.Provider:
                case PoiProperty.Country:
                case PoiProperty.City:
                //case PoiProperty.Zip:
                case PoiProperty.Latitude:
                case PoiProperty.Longitude:
                    return 10;
                    
                //case PoiProperty.ProviderPlaceId:
                case PoiProperty.Description:
                case PoiProperty.Street:
                    return 25;

                case PoiProperty.OriginalAddress:
                case PoiProperty.FormattedAddress:
                    return 42;                

                case PoiProperty.Region:
                case PoiProperty.District:
                case PoiProperty.Location:
                    return 15;
                    
                case PoiProperty.StreetNumber:
                case PoiProperty.URL_Map:
                    return 8;

                default: return 8;
            }            
        }
        private static readonly Dictionary<PoiProperty, string> DefaultNames = new Dictionary<PoiProperty, string>()
        {
            { PoiProperty.Provider, "Provider" },
            //{ PoiProperty.ProviderPlaceId, "Provider ID" },
            
            { PoiProperty.OriginalAddress, "Оригинальный запрос" },
            { PoiProperty.Description, "Описание" },

            { PoiProperty.Country, "Страна" },
            { PoiProperty.Region, "Область" },
            { PoiProperty.City, "Город" },
            { PoiProperty.District, "Район" },
            { PoiProperty.Street, "Улица" },
            { PoiProperty.StreetNumber, "Дом" },
            //{ PoiProperty.Zip, "Индекс" },
            { PoiProperty.FormattedAddress, "Адрес Poi" },

            { PoiProperty.Location, "Location" },
            { PoiProperty.Latitude, "Lat" },
            { PoiProperty.Longitude, "Lon" },

            { PoiProperty.URL_Map, "Карта" }
        };
    }
}
