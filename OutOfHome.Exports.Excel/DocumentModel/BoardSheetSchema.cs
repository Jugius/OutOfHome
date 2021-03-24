using OutOfHome.Models;
using System;

namespace OutOfHome.Exports.Excel.DocumentModels
{
    public sealed class BoardSheetSchema : SheetSchema
    {
        public bool DrawOccupation { get; set; } = true;
        public DateTimePeriod OccupationVisiblePeriod 
        {
            get => _occupationVisiblePeriod ??= new DateTimePeriod(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, 12, 31));
            set => _occupationVisiblePeriod = value;
        }
        private DateTimePeriod? _occupationVisiblePeriod;

        public DateTimePeriod PriceVisiblePeriod
        {
            get => _priceVisiblePeriod ??= new DateTimePeriod(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddSeconds(-1));
            set => _priceVisiblePeriod = value;
        }
        private DateTimePeriod? _priceVisiblePeriod;
    }
}
