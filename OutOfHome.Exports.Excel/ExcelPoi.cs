using OutOfHome.Models.Pois;
using OutOfHome.Models.Views;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    public class ExcelPoi : BasePoiModelView, IColored
    {
        public Color Color { get; set; }
        public ExcelPoi(Poi poi) : base(poi) { }
    }
}
