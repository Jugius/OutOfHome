using System;
using System.Collections.Generic;
using System.Text;

namespace OutOfHome.Imports.Excel.Models
{
    public class ExcelFileInfo
    {
        public string FilePath { get; set; }
        public int SheetIndex { get; } = 0;
        public IEnumerable<PropertySetter> Columns { get; set; }     
    }

}
