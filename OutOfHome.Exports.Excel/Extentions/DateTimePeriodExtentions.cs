using OutOfHome.Models;
using System;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.Extentions
{
    internal static class DateTimePeriodExtentions
    {
        internal static List<DateTimePeriod> SplitPeriodIntoMonthParts(this DateTimePeriod period)
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
        internal static List<DateTimePeriod> SplitPeriodIntoMonthPartsA(this DateTimePeriod period)
        {
            var Start = period.Start;
            var End = period.End;
            List<DateTimePeriod> list = new List<DateTimePeriod>();

            var runningDate = Start;
            while(runningDate < End)
            {
                var nextMonthSeed = runningDate.AddMonths(1);
                var to = Min(new DateTime(nextMonthSeed.Year, nextMonthSeed.Month, 1), End);
                list.Add(new DateTimePeriod(runningDate, to));
                runningDate = to;
            }
            return list;
        }
        private static DateTime Min(DateTime date1, DateTime date2)
        {
            return (date1 < date2 ? date1 : date2);
        }
        
    }
}

