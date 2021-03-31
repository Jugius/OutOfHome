
namespace OutOfHome.Exports.Excel.DocumentModel.Fields
{
    public interface IExcelField : IPropertyGetter
    {
        bool IsHyperlink { get; set; }
        string NumberFormat { get; }
        string ColumnHeader { get; set; }
        int ColumnWidth { get; set; }
    }
}
