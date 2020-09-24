using OutOfHome.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OutOfHome.Exports.Excel.Models
{
    public sealed class ExcelField : BoardPropertyGetter
    {       
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _columnWidth = 0;
        public string ColumnLetter
        {
            get => _columnLetter ?? string.Empty;
            set => _columnLetter = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _columnLetter;
        public ExcelField(BoardProperty property) : base(property) { }

        private static int GetDefaultColumnWidth(BoardProperty kind)
        {
            switch (kind)
            {
                case BoardProperty.ProviderID:
                case BoardProperty.SupplierCode:
                case BoardProperty.City:
                case BoardProperty.OccSource:
                    return 15;

                case BoardProperty.Provider:
                case BoardProperty.Supplier:
                    return 17;

                case BoardProperty.Address:
                    return 60;

                case BoardProperty.Side:
                case BoardProperty.Size:
                case BoardProperty.Light:
                case BoardProperty.OTS:
                case BoardProperty.GRP:
                case BoardProperty.Color:
                    return 7;

                default: return 8;
            }
        }
    }
}
