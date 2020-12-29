using OutOfHome.Models.Binds;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModels
{
    public sealed class BindExcelField : BindPropertyGetter, IExcelField
    {        
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _columnWidth = 0;
        public BindExcelField(BindProperty kind) : base(kind) { }
        public static List<BindExcelField> GetDefaultColumns() => new List<BindExcelField>(20)
            {
                new BindExcelField(BindProperty.OriginalAddress),
                new BindExcelField(BindProperty.Latitude),
                new BindExcelField(BindProperty.Longitude),
                new BindExcelField(BindProperty.Country),
                new BindExcelField(BindProperty.Region),
                new BindExcelField(BindProperty.City),
                new BindExcelField(BindProperty.District),
                new BindExcelField(BindProperty.Street),
                new BindExcelField(BindProperty.StreetNumber),
                new BindExcelField(BindProperty.Zip),
                new BindExcelField(BindProperty.Description),
                new BindExcelField(BindProperty.URL_Map),
                new BindExcelField(BindProperty.ProviderPlaceId)
        };
        private static int GetDefaultColumnWidth(BindProperty kind)
        {
            switch(kind)
            {
                case BindProperty.Provider:
                case BindProperty.Country:
                case BindProperty.City:
                case BindProperty.Zip:
                case BindProperty.Latitude:
                case BindProperty.Longitude:
                    return 10;
                    
                case BindProperty.ProviderPlaceId:
                case BindProperty.Description:
                case BindProperty.Street:
                    return 25;

                case BindProperty.OriginalAddress:
                case BindProperty.FormattedAddress:
                    return 42;                

                case BindProperty.Region:
                case BindProperty.District:
                case BindProperty.Location:
                    return 15;
                    
                case BindProperty.StreetNumber:
                case BindProperty.URL_Map:
                    return 8;

                default: return 8;
            }            
        }
    }
}
