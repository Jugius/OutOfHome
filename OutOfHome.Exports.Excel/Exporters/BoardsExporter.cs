using OfficeOpenXml;
using OutOfHome.Exports.Excel.DocumentModels;
using OutOfHome.Exports.Excel.Extentions;
using OutOfHome.Models;
using OutOfHome.Models.Boards;
using OutOfHome.Models.Boards.SupplierInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OutOfHome.Exports.Excel.DocumentModel.Fields;

namespace OutOfHome.Exports.Excel.Exporters
{
    public static class BoardsExporter
    {
        static BoardsExporter()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        
        
        public static Task<string> Export(List<ExcelBoard> boards, ExcelFileInfo fileInfo, IProgress<DataProgress> progress = null, CancellationToken cancellationToken = default)
        {
            var task = Task.Run(() => 
            {
                int _itemsTotal = boards.Count;
                BoardSheetSchema schema = fileInfo.SheetSchema as BoardSheetSchema;
                var schemaTableColumns = schema.TableColumns.ToList();
                Dictionary<IExcelField, int> columnsIndexesDic = schema.TableColumns.GetColumnsIndexesDictionary();

                int _itemsDone = 0, _prevProgress = 0;

                bool needDrawOccupation = schema.DrawOccupation && boards.Any(a => a.Occupation != null);
                List<DateTimePeriod> drawingPeriods = needDrawOccupation ? Extentions.DateTimePeriodExtentions.BuildMonthPeriods(schema.OccupationVisiblePeriod) : new List<DateTimePeriod>(0);

                //bool needDrawPrice = schemaTableColumns.Any(a => a is BoardExcelField b && b.Kind == BoardProperty.Price) && boards.Any(a => a.Price != null);
                DateTimePeriod pricePeriod = new DateTimePeriod { Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1), End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddSeconds(-1) };


                using(ExcelPackage package = new ExcelPackage())                 
                {

                    bool linksAsFormula = (schemaTableColumns.Count(a => a.IsHyperlink) * _itemsTotal) > 65500;

                    var worksheet = package.Workbook.Worksheets.Add(schema.PageName);

                    worksheet.View.ShowGridLines = false;
                    int row = 2;

                    foreach(var board in boards) 
                    {
                        foreach(var column in schemaTableColumns) 
                        {
                            var cell = worksheet.Cells[row, columnsIndexesDic[column]];
                            if(column.IsHyperlink) {
                                string link = column.GetPropertyValueFrom(board)?.ToString();

                                if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.ColumnHeader, Uri.EscapeUriString(uri.ToString()), false, true, linksAsFormula);
                            }
                            else if(column is ColorExcelField)
                            {
                                cell.SetBackgroundColor(board.Color);
                            }
                            else if(column is BoardExcelField b) 
                            { 
                                cell.Value = b.GetPropertyValueFrom(board, pricePeriod);
                            }
                        }


                        if(drawingPeriods.Count > 0) 
                        {
                            int monthColumn = schemaTableColumns.Count;

                            foreach(var period in drawingPeriods)
                            {
                                var cell = worksheet.Cells[row, ++monthColumn];

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
                        _itemsDone++;
                        row++;
                        if(progress != null)
                        {
                            int newProgress = _itemsDone * 100 / _itemsTotal;
                            if(newProgress != _prevProgress)
                            {
                                _prevProgress = newProgress;
                                progress.Report(new DataProgress(newProgress, "Выполнено: " + _itemsDone + " / " + _itemsTotal));
                            }
                        }
                    }

                    worksheet.InsertTable(_itemsTotal, columnsIndexesDic, schema, drawingPeriods);
                    try 
                    {
                        if(progress != null)
                            progress.Report(new DataProgress(100, "Завершено. Сохраняем файл..."));

                        package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));
                        return fileInfo.FilePath;
                    }
                    catch(Exception ex) {
                        throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                    }
                }
            }, cancellationToken);
            return task;        
        }
        

        public static Task Export(List<Board> boards, ExcelFileInfo fileInfo, IProgress<DataProgress> progress, CancellationToken cancellationToken = default)
        {
            var task = Task.Run(() =>
            {
                int _itemsTotal = boards.Count;
                BoardSheetSchema schema = fileInfo.SheetSchema as BoardSheetSchema;
                var schemaTableColumns = schema.TableColumns.ToList();
                Dictionary<IExcelField, int> columnsIndexesDic = schema.TableColumns.GetColumnsIndexesDictionary();

                int _itemsDone = 0, _prevProgress = 0;

                bool needDrawOccupation = schema.DrawOccupation && boards.Any(a => (a is IHaveSupplierContent));
                List<DateTimePeriod> drawingPeriods = needDrawOccupation ? Extentions.DateTimePeriodExtentions.BuildMonthPeriods(schema.OccupationVisiblePeriod) : new List<DateTimePeriod>(0);

                using (ExcelPackage package = new ExcelPackage())
                {
                    bool linksAsFormula = (schemaTableColumns.Count(a => a.IsHyperlink) * _itemsTotal) > 65500;

                    var worksheet = package.Workbook.Worksheets.Add(schema.PageName);

                    worksheet.View.ShowGridLines = false;
                    int row = 2;
                    DateTimePeriod pricePeriod = new DateTimePeriod { Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1), End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddSeconds(-1) };

                    foreach (var board in boards)
                    {
                        foreach (var column in schemaTableColumns)
                        {
                            var cell = worksheet.Cells[row, columnsIndexesDic[column]];
                            if (column.IsHyperlink)
                            {
                                string link = column.GetPropertyValueFrom(board)?.ToString();

                                if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.ColumnHeader, Uri.EscapeUriString(uri.ToString()), false, true, linksAsFormula);
                            }
                            else if(column is BoardExcelField b)
                            {
                                cell.Value = b.GetPropertyValueFrom(board, pricePeriod);
                            }
                        }

                        if(drawingPeriods.Count > 0)
                        {
                            int monthColumn = schemaTableColumns.Count;

                            foreach(var period in drawingPeriods)
                            {
                                var cell = worksheet.Cells[row, ++monthColumn];

                                if(board is IHaveSupplierContent content && content.Occupation != null)
                                {
                                    OccupationStatus status = content.Occupation.GetStatus(period.Start, period.End);

                                    if(status.Kind != OccupationKind.Free)
                                    {
                                        cell.Value = status.Value;
                                        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(status.Kind.GetCellColor());
                                    }                                    
                                }
                                else
                                {
                                    cell.Value = "Н/Д";
                                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(OccupationKind.Unavailable.GetCellColor());
                                }
                            }
                        }

                        _itemsDone++;
                        row++;
                        if (progress != null)
                        {
                            int newProgress = _itemsDone * 100 / _itemsTotal;
                            if (newProgress != _prevProgress)
                            {
                                _prevProgress = newProgress;
                                progress.Report(new DataProgress(newProgress, "Выполнено: " + _itemsDone + " / " + _itemsTotal));
                            }
                        }
                    }
                    worksheet.InsertTable(_itemsTotal, columnsIndexesDic, schema, drawingPeriods);
                    try
                    {
                        if (progress != null)
                            progress.Report(new DataProgress(100, "Завершено. Сохраняем файл..."));
                        package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                    }
                }
            }, cancellationToken);
            return task;
        }
        
        public class DataProgress
        {
            public int Progress { get; }
            public string Description { get; }
            public DataProgress(int progress, string description)
            {
                this.Progress = progress;
                this.Description = description;
            }
        }
    }
}
