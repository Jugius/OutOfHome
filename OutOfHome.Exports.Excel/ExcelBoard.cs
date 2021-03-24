using OutOfHome.Models.Boards;
using OutOfHome.Models.Boards.SupplierInfo;
using OutOfHome.Models.Pois;
using OutOfHome.Models.Views;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    public class ExcelBoard : BaseBoardModelView, IHaveSupplierContent, IColored, IBinded
    {
        public PriceInfo Price { get; set; }
        public OccupationInfo Occupation { get; set; }
        public Color Color { get; set; }
        public Poi Poi { get; set; }
        public ExcelBoard(BaseBoardModelView board) : base(board) { }
    }
}
