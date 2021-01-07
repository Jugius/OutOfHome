using OfficeOpenXml;
using OutOfHome.Imports.Excel.Models;
using OutOfHome.Models.Binds;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OutOfHome.Imports.Excel
{
    public static class Import
    {
        static Import()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public static Task<List<ExcelBoard>> ImportBoards(ExcelFileInfo fileInfo)
        {
            var task = Task.Run(() => {

                using FileStream fileStream = new FileStream(fileInfo.FilePath, FileMode.Open);
                using ExcelPackage excel = new ExcelPackage(fileStream);

                var workSheet = excel.Workbook.Worksheets[fileInfo.SheetIndex];
                IEnumerable<ExcelBoard> newcollection = workSheet.ConvertSheetToObjects(fileInfo.Columns.Cast<BoardPropertySetter>());
                return newcollection.ToList();
            });
            return task;
        }

        public static Task<IEnumerable<Bind>> ImportBinds(ExcelFileInfo fileInfo)
        {
            var task = Task.Run(() => {

                using(FileStream fileStream = new FileStream(fileInfo.FilePath, FileMode.Open))
                {
                    using(ExcelPackage excel = new ExcelPackage(fileStream))
                    {
                        var workSheet = excel.Workbook.Worksheets[fileInfo.SheetIndex];
                        IEnumerable<Bind> newcollection = workSheet.ConvertSheetToObjects(fileInfo.Columns.Cast<BindPropertySetter>()).ToList();
                        return newcollection;
                    }
                }
            });
            return task;
        }



        //public class DataProgress
        //{
        //    public int Progress { get; }
        //    public string Description { get; }
        //    public DataProgress(int progress, string description)
        //    {
        //        this.Progress = progress;
        //        this.Description = description;
        //    }
        //}
    }
    
}
