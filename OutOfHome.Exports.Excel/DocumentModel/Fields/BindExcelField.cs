using OutOfHome.Models.Binds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Exports.Excel.DocumentModel.Fields
{         
    public sealed class BindExcelField : BindPropertyGetter, IExcelField
    {
        private static readonly HashSet<BindProperty> PropertiesContainsHyperlinks = new HashSet<BindProperty>
        {
            BindProperty.Map,
        };        
        private string _columnHeader;
        public bool IsHyperlink { get; set; }

        public string NumberFormat => this.Kind switch
        {
            BindProperty.Distance => "# ##0",
            _ => null,
        };

        public string ColumnHeader
        {
            get => _columnHeader ?? DefaultNames[this.Kind];
            set => _columnHeader = string.IsNullOrEmpty(value) ? null : value;
        }
        public int ColumnWidth { get; set; } = 8;
        public BindExcelField(BindProperty property) : base(property) 
        {
            this.IsHyperlink = PropertiesContainsHyperlinks.Contains(property);
        }
        private static readonly Dictionary<BindProperty, string> DefaultNames = new Dictionary<BindProperty, string>()
        {
            { BindProperty.Distance, "расст." },
            { BindProperty.Map, "маршрут" },
        };
    }
}
