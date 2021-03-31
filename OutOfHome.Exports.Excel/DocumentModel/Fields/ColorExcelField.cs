using OutOfHome.Models.Views;

namespace OutOfHome.Exports.Excel.DocumentModel.Fields
{
    public sealed class ColorExcelField : PropertyGetter<IColored>, IExcelField
    {
        public bool IsHyperlink { get; set; } = false;
        public string NumberFormat => null;
        public string ColumnHeader { get; set; } = "цвет";
        public int ColumnWidth { get; set; } = 7;
        public override object GetPropertyValueFrom(IColored source) => source.Color;
        public override object GetPropertyValueFrom(object source) => GetPropertyValueFrom(source as IColored);
    }
}
