using OutOfHome.Models.Boards;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModels
{
    public sealed class BoardExcelField : BoardPropertyGetter, IExcelField
    {
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _columnWidth = 0;
        public BoardExcelField(BoardProperty property) : base(property) { }

        public static List<BoardExcelField> GetDefaultColumns() => new List<BoardExcelField>(20)
            {
                new BoardExcelField(BoardProperty.Supplier),
                new BoardExcelField(BoardProperty.ProviderID),
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
                new BoardExcelField(BoardProperty.URL_DoorsMap),
                new BoardExcelField(BoardProperty.URL_GoogleMapPoint),
                new BoardExcelField(BoardProperty.Price),
                new BoardExcelField(BoardProperty.DoorsId),
                new BoardExcelField(BoardProperty.OTS),
                new BoardExcelField(BoardProperty.GRP),
                new BoardExcelField(BoardProperty.Provider),
            };

        private static int GetDefaultColumnWidth(BoardProperty kind)
        {
            switch(kind)
            {
                case BoardProperty.ProviderID:
                case BoardProperty.SupplierCode:
                case BoardProperty.City:
                case BoardProperty.OccSource:
                    return 15;

                case BoardProperty.Provider:
                case BoardProperty.Supplier:
                    return 17;

                case BoardProperty.Address:
                    return 60;

                case BoardProperty.Side:
                case BoardProperty.Size:
                case BoardProperty.Light:
                case BoardProperty.OTS:
                case BoardProperty.GRP:
                case BoardProperty.Color:
                    return 7;

                default: return 8;
            }
        }
    }  
}
