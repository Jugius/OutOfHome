using System;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.Models
{
    public sealed class SheetSchema
    {
        public System.Drawing.Font Font { get; set; } = new System.Drawing.Font("Calibri", 10);
        public System.Drawing.Color FontColor { get; set; } = System.Drawing.Color.FromArgb(30, 55, 55);
        public string PageName { get; set; } = "Лист1";
        public List<Field> TableColumns { get; set; }
        public System.Drawing.Color TableCaptionColor { get; set; } = System.Drawing.Color.FromArgb(153, 180, 209);

        public static SheetSchema CreateDefault()
        {
            SheetSchema s = new SheetSchema();
            List<Field> f = new List<Field>(20)
            {
                new Field(FieldKind.Supplier),
                new Field(FieldKind.ProviderID),
                new Field(FieldKind.SupplierCode),
                new Field(FieldKind.City),
                new Field(FieldKind.Address),
                new Field(FieldKind.Kind),
                new Field(FieldKind.Size),
                new Field(FieldKind.Side),
                new Field(FieldKind.Light),
                new Field(FieldKind.URL_Photo){ IsHyperlink = true },
                new Field(FieldKind.URL_Map){ IsHyperlink = true },
                new Field(FieldKind.Price),
                new Field(FieldKind.DoorsId),
                new Field(FieldKind.OTS),
                new Field(FieldKind.GRP),
                new Field(FieldKind.Provider),
                //new Field(FieldKind.OccSource),
                //new Field(FieldKind.Location)
            };

            int index = 0;
            foreach (var col in f)
                col.ColumnLetter = GetColumnLetter(++index);

            s.TableColumns = f;
            return s;
        }
        internal static string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }
    }
}
