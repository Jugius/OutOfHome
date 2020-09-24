using OutOfHome.Models;
using OutOfHome.Models.Occupation;
using OutOfHome.Models.Views;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    public class ExcelBoard : BaseBoardModelView, IHaveSupplierContent, IColored
    {
        public int Price { get; set; }
        public OccupationInfo Occupation { get; set; }
        public Color Color { get; set; }

        public ExcelBoard(BaseBoardModelView board) : base(board)
        {

        }

    }
}
