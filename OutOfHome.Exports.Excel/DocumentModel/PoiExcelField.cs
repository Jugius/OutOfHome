using OutOfHome.Models.Pois;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModels
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
    }
}
