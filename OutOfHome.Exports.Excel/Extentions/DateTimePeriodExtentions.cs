using OutOfHome.Models;
using System;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.Extentions
{
    internal static class DateTimePeriodExtentions
    {
        internal static List<DateTimePeriod> BuildMonthPeriods(this DateTimePeriod period)
        {
            if(period.Start.Month == period.End.Month && period.Start.Year == period.End.Year)
                return new List<DateTimePeriod>(1) { period };

            DateTime first = period.Start;
            DateTime last = new DateTime(first.Year, first.Month, 1).AddMonths(1).AddDays(-1);

            List<DateTimePeriod> periods = new List<DateTimePeriod>() { new DateTimePeriod(first, last) };

            while(last.Month != period.End.Month || last.Year != period.End.Year)
            {
                first = new DateTime(first.Year, first.Month, 1).AddMonths(1);
                last = first.AddMonths(1).AddDays(-1);
                if(last > period.End)
                    last = period.End;

                periods.Add(new DateTimePeriod(first, last));
            }

            return periods;
        }
    }
}
