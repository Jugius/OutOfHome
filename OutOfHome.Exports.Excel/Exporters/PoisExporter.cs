using OfficeOpenXml;
using OfficeOpenXml.Table;
using OutOfHome.Exports.Excel.DocumentModel.Fields;
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
        private const string TableName = "Pois";
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
                var columns = schema.TableColumns.ToList();
                Dictionary<IExcelField, int> dic = columns.GetColumnsIndexesDictionary();
                foreach(var poi in pois)
                {
                    foreach(var column in columns)
                    {
                        var cell = worksheet.Cells[row, dic[column]];
                        if(column.IsHyperlink)
                        {
                            string link = column.GetPropertyValueFrom(poi)?.ToString();

                            if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                cell.WriteHyperlink(column.ColumnHeader, uri.ToString());
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
                worksheet.InsertTable(row - 2, dic, schema, new List<Models.DateTimePeriod>(0), TableName);

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
