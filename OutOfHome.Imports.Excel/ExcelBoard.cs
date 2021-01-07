using OutOfHome.Models.Boards;
using OutOfHome.Models.Boards.SupplierInfo;
using OutOfHome.Models.Views;

namespace OutOfHome.Imports.Excel
{
    public class ExcelBoard : Board, IHaveSupplierContent, IColored
    {
        public PriceInfo Price { get; set; }
        public OccupationInfo Occupation { get; set; }
        public System.Drawing.Color Color { get; set; }
    }
}
