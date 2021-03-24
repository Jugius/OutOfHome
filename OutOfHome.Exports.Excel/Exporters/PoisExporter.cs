using OfficeOpenXml;
using OfficeOpenXml.Table;
using OutOfHome.Exports.Excel.DocumentModels;
using OutOfHome.Exports.Excel.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutOfHome.Exports.Excel.Exporters
{
    public static class PoisExporter
    {
        static PoisExporter()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static async Task ExportAsync(IEnumerable<ExcelPoi> pois, DocumentModels.ExcelFileInfo fileInfo)
        {
            await Task.Run(() => { Export(pois, fileInfo); }).ConfigureAwait(false);
        }
        public static void Export(IEnumerable<ExcelPoi> pois, DocumentModels.ExcelFileInfo fileInfo)
        {
            using(ExcelPackage package = new ExcelPackage())
            {
                SheetSchema schema = fileInfo.SheetSchema;

                var worksheet = package.Workbook.Worksheets.Add(schema.PageName);
                worksheet.View.ShowGridLines = false;

                int row = 2;
                var columns = schema.TableColumns.Cast<PoiExcelField>().ToList();
                Dictionary<PoiExcelField, int> dic = GetColumnsDictionary(columns);
                foreach(var poi in pois)
                {
                    foreach(var column in columns)
                    {
                        var cell = worksheet.Cells[row, dic[column]];
                        if(column.IsHyperlink)
                        {
                            string link = column.GetPropertyValueFrom(poi)?.ToString();

                            if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                cell.WriteHyperlink(column.Name, uri.ToString());
                        }
                        else
                        {
                            cell.Value = column.GetPropertyValueFrom(poi);
                            
                            if(poi.Color != System.Drawing.Color.Empty)
                                cell.Style.Font.Color.SetColor(poi.Color);
                        }
                    }
                    row++;
                }
                worksheet.InsertTable(row - 2, dic, schema);

                try
                {
                    package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));

                }
                catch(Exception ex)
                {
                    throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                }
            }
        }
        private static void InsertTable(this ExcelWorksheet worksheet, int rowsNumber, Dictionary<PoiExcelField, int> dic, SheetSchema schema)
        {
            foreach(var pair in dic)
            {
                worksheet.Cells[1, pair.Value].Value = pair.Key.Name;
                var col = worksheet.Column(pair.Value);
                col.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                col.Width = pair.Key.ColumnWidth;
                if(pair.Key.NumberFormat != null)
                    col.Style.Numberformat.Format = pair.Key.NumberFormat;
            }
            ExcelRange rg = worksheet.Cells[1, 1, rowsNumber + 1, dic.Count];
            rg.FormatRange(schema.FontColor, schema.Font);
            rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            const string tableName = "Pois";

            ExcelTable tab = worksheet.Tables.Add(rg, tableName);
            tab.TableStyle = TableStyles.Light9;
            rg.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        private static Dictionary<PoiExcelField, int> GetColumnsDictionary(List<PoiExcelField> tableColumns)
        {
            Dictionary<PoiExcelField, int> dic = new Dictionary<PoiExcelField, int>(tableColumns.Count);
            int column = 0;
            foreach(var c in tableColumns)
            {
                dic.Add(c, ++column);
            }
            return dic;
        }
        private static void FormatRange(this ExcelRange range, System.Drawing.Color foreColor, System.Drawing.Font font)
        {
            range.Style.Font.Name = font.Name;
            range.Style.Font.Size = font.Size;
            range.Style.Font.Bold = font.Bold;
            range.Style.Font.Italic = font.Italic;
            //range.Style.Font.Color.SetColor(foreColor);
        }
    }
}
