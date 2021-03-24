
namespace OutOfHome.Exports.Excel.DocumentModels
{
    public interface IExcelField
    {
        bool IsHyperlink { get; set; }
        int ColumnWidth { get; set; }
    }
}
