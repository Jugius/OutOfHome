using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OutOfHome.Exports.Excel.Models;
using OutOfHome.Models.Occupation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    internal static class ExcelExtentions
    {
        internal static int InsertTable(this ExcelWorksheet worksheet, int rowsNumber, SheetSchema schema)
        {
            const int headerRow = 1;
            var columns = schema.TableColumns;

            foreach (var field in columns)
            {
                var columnLetter = field.ColumnLetter;
                worksheet.Cells[columnLetter + headerRow.ToString()].Value = field.Name;
                var col = worksheet.Column(columns.IndexOf(field) + 1);
                col.Style.HorizontalAlignment = field.GetDefaultAlign();
                col.Width = field.Width;
                if (field.NumberFormat != null)
                    col.Style.Numberformat.Format = field.NumberFormat;
            }

            int column = columns.Count;
            int month = DateTime.Now.Month;
            while (month <= 12)
            {
                var columnLetter = SheetSchema.GetColumnLetter(++column);
                worksheet.Cells[columnLetter + headerRow.ToString()].Value = GetMonthName(month);
                worksheet.Column(column).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(column).Width = 7;
                month++;
            }

            const int firstRow = 1, firstColumn = 1;
            int lastRow = headerRow + rowsNumber;
            int lastColumn = column;
            ExcelRange rg = worksheet.Cells[firstRow, firstColumn, lastRow, lastColumn];
            rg.FormatRange(schema.FontColor, schema.Font);
            rg.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            const string tableName = "Grid";

            ExcelTable tab = worksheet.Tables.Add(rg, tableName);
            tab.TableStyle = TableStyles.Light9;
            rg.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            return headerRow + 1;
        }
        public static void WriteHyperlink(this ExcelRange cell, string text, string url, bool excelHyperlink = false, bool underline = true, bool asFormula = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (excelHyperlink)
                cell.Hyperlink = new ExcelHyperLink(url) { Display = text };
            else
            {
                if (asFormula)
                {
                    cell.Formula = "HYPERLINK(\"" + url + "\",\"" + text + "\")";
                    //cell.Formula = string.Format("HYPERLINK(\"{0}\",\"{1}\")", url, text);
                }
                else
                {
                    cell.Hyperlink = new Uri(url);
                    cell.Value = text;                    
                }
                cell.Style.Font.UnderLine = underline;
                cell.Style.Font.Color.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Hyperlink);
            }       
        }


        private static void FormatRange(this ExcelRange range, System.Drawing.Color foreColor, System.Drawing.Font font)
        {
            range.Style.Font.Name = font.Name;
            range.Style.Font.Size = font.Size;
            range.Style.Font.Bold = font.Bold;
            range.Style.Font.Italic = font.Italic;
            //range.Style.Font.Color.SetColor(foreColor);
        }
        public static ExcelHorizontalAlignment GetDefaultAlign(this Field field)
        {
            switch (field.Kind)
            {
                case FieldKind.Side:
                case FieldKind.Size:
                case FieldKind.Light:
                case FieldKind.URL_Map:
                case FieldKind.URL_Photo:
                case FieldKind.OTS:
                case FieldKind.GRP:
                    return ExcelHorizontalAlignment.Center;
                default: return ExcelHorizontalAlignment.Left;
            }
        }
        private static readonly Dictionary<int, string> Months = new Dictionary<int, string>
        {
            { 1, "янв" },
            { 2, "фев" },
            { 3, "мар" },
            { 4, "апр" },
            { 5, "май" },
            { 6, "июн" },
            { 7, "июл" },
            { 8, "авг" },
            { 9, "сен" },
            { 10, "окт" },
            { 11, "ноя" },
            { 12, "дек" },
        };
        internal static string GetMonthName(int index) => Months[index];

        private static readonly Dictionary<OccupationKind, Color> OccupationColors = new Dictionary<OccupationKind, Color>
        {
            { OccupationKind.Booked, Color.FromArgb(255, 199, 206) },
            { OccupationKind.Reserved, Color.FromArgb(255, 235, 156) },
            { OccupationKind.Unavailable, Color.FromArgb(242, 242, 242) }
        };
        internal static Color GetCellColor(this OccupationKind kind) => OccupationColors.TryGetValue(kind, out Color color) ? color : Color.Transparent;
    }
}
