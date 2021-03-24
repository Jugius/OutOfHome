using OutOfHome.Models.Boards;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModels
{
    public sealed class BoardExcelField : BoardPropertyGetter, IExcelField
    {
        private static readonly HashSet<BoardProperty> PropertiesContainsHyperlinks = new HashSet<BoardProperty>
        {
            BoardProperty.URL_DoorsMap, BoardProperty.URL_DoorsPhoto, BoardProperty.URL_GoogleMapPoint, BoardProperty.URL_Map, BoardProperty.URL_Photo, BoardProperty.URL_StreetsView, BoardProperty.BindMap
        };
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _columnWidth = 0;
        public BoardExcelField(BoardProperty property) : base(property)
        {
            this.IsHyperlink = PropertiesContainsHyperlinks.Contains(property);
        }

        public static List<BoardExcelField> GetDefaultColumns() => new List<BoardExcelField>(20)
            {
                new BoardExcelField(BoardProperty.Supplier),
                new BoardExcelField(BoardProperty.SupplierCode),
                new BoardExcelField(BoardProperty.Region),
                new BoardExcelField(BoardProperty.City),
                new BoardExcelField(BoardProperty.Address),
                new BoardExcelField(BoardProperty.Kind),
                new BoardExcelField(BoardProperty.Size),
                new BoardExcelField(BoardProperty.Side),
                new BoardExcelField(BoardProperty.Light),
                new BoardExcelField(BoardProperty.URL_Photo),
                new BoardExcelField(BoardProperty.URL_Map),
                new BoardExcelField(BoardProperty.URL_DoorsPhoto),                
                new BoardExcelField(BoardProperty.URL_GoogleMapPoint),
                new BoardExcelField(BoardProperty.Price),
                new BoardExcelField(BoardProperty.DoorsId),
                new BoardExcelField(BoardProperty.OTS),
                new BoardExcelField(BoardProperty.GRP),
                new BoardExcelField(BoardProperty.Provider),
            };

        private static int GetDefaultColumnWidth(BoardProperty kind)
        {
            return kind switch
            {
                BoardProperty.ProviderID or BoardProperty.SupplierCode or BoardProperty.City or BoardProperty.OccSource => 15,
                BoardProperty.Provider or BoardProperty.Supplier => 17,
                BoardProperty.Address or BoardProperty.BindAddress or BoardProperty.BindDescription => 60,
                BoardProperty.Side or BoardProperty.Size or BoardProperty.Light or BoardProperty.OTS or BoardProperty.GRP or BoardProperty.Color => 7,
                _ => 8,
            };
        }
    }  
}
