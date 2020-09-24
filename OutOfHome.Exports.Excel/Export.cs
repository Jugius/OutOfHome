using OfficeOpenXml;
using OutOfHome.Exports.Excel.Models;
using OutOfHome.Models;
using OutOfHome.Models.Occupation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OutOfHome.Exports.Excel
{
    public static class Export
    {
        static Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static Task ExportBoards(List<ExcelBoard> boards, ExcelFileInfo fileInfo)
        {
            var task = Task.Run(() => 
            {
                int _itemsTotal = boards.Count;
                bool needDrawOccupation = boards.Any(a => a.Occupation != null);
                using(ExcelPackage package = new ExcelPackage())                 
                {
                    SheetSchema schema = fileInfo.SheetSchema;
                    var linksNumber = schema.TableColumns.Count(a => a.IsHyperlink) * _itemsTotal;

                    bool linksAsFormula = (schema.TableColumns.Count(a => a.IsHyperlink) * _itemsTotal) > 65500;

                    var worksheet = package.Workbook.Worksheets.Add(schema.PageName);

                    worksheet.View.ShowGridLines = false;
                    int row = 2;

                    foreach(var board in boards) 
                    {
                        foreach(var column in schema.TableColumns) 
                        {
                            var cell = worksheet.Cells[column.ColumnLetter + row.ToString()];
                            if(column.IsHyperlink) {
                                string link = column.GetPropertyValueFrom(board)?.ToString();

                                if(!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.Name, uri.ToString(), false, true, linksAsFormula);
                            }
                            else if(column.Kind == BoardProperty.Color)
                            {
                                cell.SetBackgroundColor(board.Color);
                            }
                            else {
                                cell.Value = column.GetPropertyValueFrom(board);
                            }
                        }


                        if(needDrawOccupation) 
                        {
                            int monthColumn = schema.TableColumns.Count;
                            int month = DateTime.Now.Month;
                            while(month <= 12) 
                            {
                                monthColumn++;
                                var cell = worksheet.Cells[SheetSchema.GetColumnLetter(monthColumn) + row.ToString()];
                                if(board.Occupation == null) 
                                {
                                    var columnLetter = SheetSchema.GetColumnLetter(monthColumn);
                                    cell.Value = "Н/Д";
                                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(OccupationKind.Unavailable.GetCellColor());
                                }
                                else 
                                {
                                    OccupationStatus status = board.Occupation.GetStatus(month);

                                    if(status.Kind != OccupationKind.Free) {
                                        var columnLetter = SheetSchema.GetColumnLetter(monthColumn);
                                        cell.Value = status.Value;
                                        cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(status.Kind.GetCellColor());
                                    }
                                }
                                month++;
                            }
                        }
                        row++;
                    }

                    worksheet.InsertTable(_itemsTotal, schema, needDrawOccupation);
                    try 
                    {
                        package.SaveAs(new System.IO.FileInfo(fileInfo.FilePath));

                    }
                    catch(Exception ex) {
                        throw new Exception("Ошибка сохранения файла: " + fileInfo.FilePath, ex);
                    }

                }
            });
            return task;        
        }
        public static Task ExportBoards(List<Board> boards, ExcelFileInfo fileInfo, IProgress<DataProgress> progress, CancellationToken cancellationToken = default)
        {
            var task = Task.Run(() =>
            {
                int _itemsTotal = boards.Count;
                int _itemsDone = 0, _prevProgress = 0;

                using (ExcelPackage package = new ExcelPackage())
                {
                    SheetSchema schema = fileInfo.SheetSchema;
                    var linksNumber = schema.TableColumns.Count(a => a.IsHyperlink) * _itemsTotal;

                    bool linksAsFormula = (schema.TableColumns.Count(a => a.IsHyperlink) * _itemsTotal) > 65500;

                    var worksheet = package.Workbook.Worksheets.Add("Сетка");

                    worksheet.View.ShowGridLines = false;
                    int row = 2;

                    foreach (var board in boards)
                    {
                        foreach (var column in schema.TableColumns)
                        {
                            var cell = worksheet.Cells[column.ColumnLetter + row.ToString()];
                            if (column.IsHyperlink)
                            {
                                string link = column.GetPropertyValueFrom(board)?.ToString();

                                if (!string.IsNullOrEmpty(link) && Uri.TryCreate(link, UriKind.Absolute, out Uri uri))
                                    cell.WriteHyperlink(column.Name, uri.ToString(), false, true, linksAsFormula);
                            }
                            else
                            {
                                cell.Value = column.GetPropertyValueFrom(board);
                            }
                        }

                        int monthColumn = schema.TableColumns.Count;
                        int month = DateTime.Now.Month;
                        while (month <= 12)
                        {
                            monthColumn++;
                            var cell = worksheet.Cells[SheetSchema.GetColumnLetter(monthColumn) + row.ToString()];
                            if (!(board is IHaveSupplierContent supplierContent) || supplierContent.Occupation == null)
                            {
                                var columnLetter = SheetSchema.GetColumnLetter(monthColumn);
                                cell.Value = "Н/Д";
                                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(OccupationKind.Unavailable.GetCellColor());
                            }
                            else
                            {
                                OccupationStatus status = supplierContent.Occupation.GetStatus(month);
                                if (status.Kind != OccupationKind.Free)
                                {
                                    var columnLetter = SheetSchema.GetColumnLetter(monthColumn);
                                    cell.Value = status.Value;
                                    cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(status.Kind.GetCellColor());
                                }
                            }
                            month++;
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
                    worksheet.InsertTable(_itemsTotal, schema, true);
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
            });
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
