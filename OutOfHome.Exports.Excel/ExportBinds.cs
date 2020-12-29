using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OutOfHome.Exports.Excel.DocumentModels;
using OutOfHome.Models.Binds;

namespace OutOfHome.Exports.Excel
{
    public static class ExportBinds
    {
        static ExportBinds()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static Task Export(List<Bind> binds, DocumentModels.ExcelFileInfo fileInfo)
        {
            return Task.Run(()=> 
            {
                using(ExcelPackage package = new ExcelPackage())
                {
                    SheetSchema schema = fileInfo.SheetSchema;
                    
                    var worksheet = package.Workbook.Worksheets.Add(schema.PageName);
                    worksheet.View.ShowGridLines = false;
                    
                    int row = 2;
                    var columns = schema.TableColumns.Cast<BindExcelField>().ToList();
                    Dictionary<BindExcelField, int> dic = GetColumnsDictionary(columns);
                    foreach(var bind in binds)
                    {
                        foreach(var column in columns)
                        {                            
                            var cell = worksheet.Cells[row, dic[column]];
                            if(column.IsHyperlink)
                            {
                                string link = column.GetPropertyValueFrom(bind)?.ToString();

                                if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.Name, uri.ToString());
                            }
                            else
                            {
                                cell.Value = column.GetPropertyValueFrom(bind);
                            }
                        }
                        row++;
                    }

                    worksheet.InsertTable(binds.Count, dic, schema);

                    try
                    {
                        package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));

                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                    }
                }
            });
        }
        private static void InsertTable(this ExcelWorksheet worksheet, int rowsNumber, Dictionary<BindExcelField, int> dic, SheetSchema schema)
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
            ExcelRange rg = worksheet.Cells[1, 1, rowsNumber +1, dic.Count];
            rg.FormatRange(schema.FontColor, schema.Font);
            rg.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            const string tableName = "Binds";

            ExcelTable tab = worksheet.Tables.Add(rg, tableName);
            tab.TableStyle = TableStyles.Light9;
            rg.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        private static Dictionary<BindExcelField, int> GetColumnsDictionary(List<BindExcelField> tableColumns)
        {
            Dictionary<BindExcelField, int> dic = new Dictionary<BindExcelField, int>(tableColumns.Count);
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
