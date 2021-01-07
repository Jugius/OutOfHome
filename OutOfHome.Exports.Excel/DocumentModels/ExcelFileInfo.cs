
namespace OutOfHome.Exports.Excel.DocumentModels
{
    public class ExcelFileInfo
    {
        public string FilePath { get; set; }
        public SheetSchema SheetSchema { get; set; }
        public override string ToString() => this.FilePath;
    }
}
