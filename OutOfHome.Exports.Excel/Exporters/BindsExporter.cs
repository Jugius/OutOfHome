using OfficeOpenXml;
using OutOfHome.Exports.Excel.DocumentModels;
using OutOfHome.Exports.Excel.Extentions;
using OutOfHome.Exports.Excel.DocumentModel.Fields;
using OutOfHome.Models;
using OutOfHome.Models.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OutOfHome.Models.Boards.SupplierInfo;

namespace OutOfHome.Exports.Excel.Exporters
{
    public static class BindsExporter
    {
        private const string TableName = "Binds";
        static BindsExporter()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }


        public static Task<string> Export(List<ExcelBind> binds, ExcelFileInfo fileInfo)
        {
            var task = Task.Run(() => 
            {
                int _itemsTotal = binds.Count;
                var boards = binds.Select(a => a.Board as ExcelBoard);
                BoardSheetSchema schema = fileInfo.SheetSchema as BoardSheetSchema;
                Dictionary<IExcelField, int> columnsIndexesDic = GetColumnsDictionary(schema.TableColumns);

                bool needDrawOccupation = schema.DrawOccupation && boards.Any(a => a.Occupation != null);
                List<DateTimePeriod> drawingPeriods = needDrawOccupation ? Extentions.DateTimePeriodExtentions.SplitPeriodIntoMonthParts(schema.OccupationVisiblePeriod) : new List<DateTimePeriod>(0);

                bool needDrawPrice = schema.TableColumns.Any(a => a is BoardPropertyGetter g && g.Kind == BoardProperty.Price) && boards.Any(a => a.Price != null);
                DateTimePeriod pricePeriod = new DateTimePeriod { Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1), End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddSeconds(-1) };

                using(ExcelPackage package = new ExcelPackage())
                {
                    bool linksAsFormula = (schema.TableColumns.Count(a => a.IsHyperlink) * _itemsTotal) > 65500;

                    var worksheet = package.Workbook.Worksheets.Add(schema.PageName);

                    worksheet.View.ShowGridLines = false;
                    int row = 2;

                    foreach(var bind in binds)
                    {
                        foreach(var column in schema.TableColumns)
                        {
                            var cell = worksheet.Cells[row, columnsIndexesDic[column]];
                            if(column.IsHyperlink)
                            {
                                string link = GetPropertyValueFrom(column, bind, pricePeriod)?.ToString();

                                if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.ColumnHeader, Uri.EscapeUriString(uri.ToString()), false, true, linksAsFormula);
                            }
                            else
                            {
                                cell.Value = GetPropertyValueFrom(column, bind, pricePeriod);
                            }
                        }


                        if(drawingPeriods.Count > 0)
                        {
                            int monthColumn = columnsIndexesDic.Count;

                            foreach(var period in drawingPeriods)
                            {
                                var cell = worksheet.Cells[row, ++monthColumn];

                                ExcelBoard board = bind.Board as ExcelBoard;

                                if(board.Occupation == null)
                                {
                                    cell.Value = "Н/Д";
                                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(OccupationKind.Unavailable.GetCellColor());
                                }
                                else
                                {
                                    OccupationStatus status = board.Occupation.GetStatus(period.Start, period.End);

                                    if(status.Kind != OccupationKind.Free)
                                    {
                                        cell.Value = status.Value;
                                        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(status.Kind.GetCellColor());
                                    }
                                }
                            }
                        }


                        row++;
                    }

                    worksheet.InsertTable(_itemsTotal, columnsIndexesDic, schema, drawingPeriods, TableName);
                    try
                    {
                        package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));
                        return fileInfo.FilePath;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                    }
                }
                
            });
            return task;
        }
        public static object GetPropertyValueFrom(IExcelField field, ExcelBind bind, DateTimePeriod pricePeriod) => field switch
        {
            BoardPropertyGetter b => b.GetPropertyValueFrom(bind.Board, pricePeriod),
            PoiPropertyGetter p => p.GetPropertyValueFrom(bind.Poi),
            BindPropertyGetter bp => bp.GetPropertyValueFrom(bind),

            _ => throw new Exception($"Ошибка получения свойства из PropertyGetter"),
        };
        
        private static Dictionary<IExcelField, int> GetColumnsDictionary(IEnumerable<IExcelField> tableColumns)
        {
            Dictionary<IExcelField, int> dic = new Dictionary<IExcelField, int>();
            int column = 0;
            foreach(var c in tableColumns)
            {
                dic.Add(c, ++column);
            }
            return dic;
        }
    }
}
