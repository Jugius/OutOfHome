using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OutOfHome.Exports.Excel.DocumentModels;
using OutOfHome.Models;
using OutOfHome.Models.Boards;
using OutOfHome.Models.Boards.SupplierInfo;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OutOfHome.Exports.Excel
{
    internal static class ExcelExtentions
    {
        internal static int InsertTable(this ExcelWorksheet worksheet, int rowsNumber, Dictionary<BoardExcelField, int> dic, SheetSchema schema, List<DateTimePeriod> drawingPeriods)
        {
            const int headerRow = 1;

            foreach(var pair in dic)
            {
                worksheet.Cells[headerRow, pair.Value].Value = pair.Key.Name;
                var col = worksheet.Column(pair.Value);
                col.Style.HorizontalAlignment = pair.Key.GetDefaultAlign();
                col.Width = pair.Key.ColumnWidth;
                if (pair.Key.NumberFormat != null)
                    col.Style.Numberformat.Format = pair.Key.NumberFormat;
            }
            int column = dic.Count;

            if(drawingPeriods.Count > 0)
            {
                foreach(var period in drawingPeriods)
                {
                    worksheet.Cells[headerRow, ++column].Value = Months[period.Start.Month] + '.' + period.Start.ToString("yy");
                    worksheet.Column(column).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(column).Width = 7;
                }
            }
            
            ExcelRange rg = worksheet.Cells[1, 1, rowsNumber + 1, column];
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
        internal static void SetBackgroundColor(this ExcelRange range, System.Drawing.Color color)
        {
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(color);
        }
        public static ExcelHorizontalAlignment GetDefaultAlign(this BoardPropertyGetter field)
        {
            switch (field.Kind)
            {
                case BoardProperty.Side:
                case BoardProperty.Size:
                case BoardProperty.Light:
                case BoardProperty.URL_Map:
                case BoardProperty.URL_Photo:
                case BoardProperty.OTS:
                case BoardProperty.GRP:
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
