using System;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.Models
{
    public sealed class SheetSchema
    {
        public System.Drawing.Font Font { get; set; } = new System.Drawing.Font("Calibri", 10);
        public System.Drawing.Color FontColor { get; set; } = System.Drawing.Color.FromArgb(30, 55, 55);
        public string PageName { get; set; } = "Лист1";
        public List<ExcelField> TableColumns { get; set; }
        public System.Drawing.Color TableCaptionColor { get; set; } = System.Drawing.Color.FromArgb(153, 180, 209);        

        public static SheetSchema CreateDefault()
        {
            SheetSchema s = new SheetSchema();
            List<ExcelField> fields = new List<ExcelField>(20)
            {
                new ExcelField(BoardProperty.Supplier),
                new ExcelField(BoardProperty.ProviderID),
                new ExcelField(BoardProperty.SupplierCode),
                new ExcelField(BoardProperty.Region),
                new ExcelField(BoardProperty.City),
                new ExcelField(BoardProperty.Address),
                new ExcelField(BoardProperty.Kind),
                new ExcelField(BoardProperty.Size),
                new ExcelField(BoardProperty.Side),
                new ExcelField(BoardProperty.Light),
                new ExcelField(BoardProperty.URL_Photo),
                new ExcelField(BoardProperty.URL_Map),
                new ExcelField(BoardProperty.URL_DoorsPhoto),
                new ExcelField(BoardProperty.URL_DoorsMap),
                new ExcelField(BoardProperty.URL_GoogleMapPoint),
                new ExcelField(BoardProperty.Price),
                new ExcelField(BoardProperty.DoorsId),
                new ExcelField(BoardProperty.OTS),
                new ExcelField(BoardProperty.GRP),
                new ExcelField(BoardProperty.Provider),
            };
            
            s.TableColumns = fields;
            s.UpdateColumnsLetters();
            return s;
        }
        public void UpdateColumnsLetters()
        {
            int index = 0;
            foreach(var column in this.TableColumns) {
                column.ColumnLetter = GetColumnLetter(++index);
            }
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
