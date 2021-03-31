using OutOfHome.Exports.Excel.DocumentModel.Fields;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModels
{
    public class SheetSchema
    {
        public System.Drawing.Font Font { get; set; } = new System.Drawing.Font("Calibri", 10);
        public System.Drawing.Color FontColor { get; set; } = System.Drawing.Color.FromArgb(30, 55, 55);
        public string PageName { get; set; } = "Лист1";
        public IEnumerable<IExcelField> TableColumns { get; set; }
        public System.Drawing.Color TableCaptionColor { get; set; } = System.Drawing.Color.FromArgb(153, 180, 209);        
    }
}
