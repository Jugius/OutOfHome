
using OutOfHome.Models.Views;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    public class ExcelBind : BaseBindModelView, IColored
    {
        private ExcelBoard board;
        private ExcelPoi poi;
        public override BaseBoardModelView Board { get => this.board; set => this.board = value as ExcelBoard; }
        public override BasePoiModelView Poi { get => this.poi; set => this.poi = value as ExcelPoi; }
        public Color Color { get; set; }        
        
    }
}
