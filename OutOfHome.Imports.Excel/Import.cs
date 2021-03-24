using OfficeOpenXml;
using OutOfHome.Imports.Excel.Models;
using OutOfHome.Models;
using OutOfHome.Models.Pois;
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

        public static async Task<IEnumerable<Poi>> ImportPoisAsync(ExcelFileInfo fileInfo)
        {
            var task = Task.Run(() => { return ImportPois(fileInfo); });
            return await task.ConfigureAwait(false);
        }
        public static IEnumerable<Poi> ImportPois(ExcelFileInfo fileInfo)
        {
            bool isSimple = fileInfo.Columns.Count() == 3;

            using FileStream fileStream = new FileStream(fileInfo.FilePath, FileMode.Open);
            using ExcelPackage excel = new ExcelPackage(fileStream);
            var workSheet = excel.Workbook.Worksheets[0];
            var rows = workSheet.Cells.Select(cell => cell.Start.Row).Distinct().OrderBy(x => x);

            foreach(var row in rows.Skip(1))
            {
                var cell = workSheet.Cells[row, 1];
                if(cell == null) throw new System.Exception($"Нулевая ячейка для столбца Координаты");

                string coords = cell.GetValue<string>();
                if(string.IsNullOrEmpty(coords))
                    throw new System.Exception($"Недопустимо пустое значение ячейки для столбца Координаты. Ячейка {cell.Address}");

                if(!Location.TryParse(coords, out Location loc))
                    throw new System.Exception($"Ошибка парсинга координат. Ячейка {cell.Address}");
                //Location loc = Location.t Parse(coords);

                cell = workSheet.Cells[row, 2];
                if(cell == null) throw new System.Exception($"Нулевая ячейка для столбца Адрес");
                string adr = cell.GetValue<string>();
                if(string.IsNullOrEmpty(adr))
                    throw new System.Exception($"Недопустимо пустое значение ячейки для столбца Адрес. Ячейка {cell.Address}");

                cell = workSheet.Cells[row, 3];
                if(cell == null) throw new System.Exception($"Нулевая ячейка для столбца Описание");
                string descript = cell.GetValue<string>();
                if(string.IsNullOrEmpty(descript))
                    descript = string.Empty;

                yield return new Poi(adr, loc, "Excel") { Description = descript };

            }                
        }
        public static IEnumerable<KeyValuePair<string, string>> GetStringPairs(string filePath)
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open);
            using ExcelPackage excel = new ExcelPackage(fileStream);
            var workSheet = excel.Workbook.Worksheets[0];
            var rows = workSheet.Cells.Select(cell => cell.Start.Row).Distinct().OrderBy(x => x);
            foreach(var row in rows.Skip(1))
            {
                var cell = workSheet.Cells[row, 1];
                if(cell == null) throw new System.Exception($"Нулевая ячейка для столбца Адрес");

                string adr = cell.GetValue<string>();
                if(string.IsNullOrEmpty(adr))
                    throw new System.Exception($"Недопустимо пустое значение ячейки для столбца Адрес. Ячейка {cell.Address}");

                cell = workSheet.Cells[row, 2];
                if(cell == null) throw new System.Exception($"Нулевая ячейка для столбца Описание");
                string descript = cell.GetValue<string>();
                if(string.IsNullOrEmpty(descript))
                    descript = string.Empty;

                yield return new KeyValuePair<string, string>(adr, descript);
            }
            
        }
        public static async Task<IEnumerable<KeyValuePair<string, string>>> GetStringPairsAsync(string filePath)
        {
            var task = Task.Run(() => { return GetStringPairs(filePath); });
            return await task.ConfigureAwait(false);
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
