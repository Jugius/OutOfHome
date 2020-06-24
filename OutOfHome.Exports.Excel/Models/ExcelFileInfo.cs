
namespace OutOfHome.Exports.Excel.Models
{
    public class ExcelFileInfo
    {
        public string FilePath { get; set; }
        public SheetSchema SheetSchema { get; set; }
        public ExcelFileInfo(string filePath) : this(filePath, null) { }
        public ExcelFileInfo(string filePath, SheetSchema sheetSchema)
        {
            this.FilePath = filePath;
            this.SheetSchema = sheetSchema ?? SheetSchema.CreateDefault();
        }
        public override string ToString() => this.FilePath;
    }
}
