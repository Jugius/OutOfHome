using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutOfHome.Models.Occupation
{
    public class OccupationInfo
    {
        public List<OccupationPeriod> OccupationPeriods { get; } = new List<OccupationPeriod>();
        public virtual OccupationStatus GetStatus(DateTime firstDay, DateTime lastDay)
        {
            if (OccupationPeriods == null || OccupationPeriods.Count == 0) return new OccupationStatus { Kind = OccupationKind.Free };          

            var periods = OccupationPeriods.Where(a => !(a.Begin > lastDay || a.End < firstDay)).ToList();

            if (periods.Count == 0)
                return new OccupationStatus { Kind = OccupationKind.Free };

            if (periods.Count == 1)
                return new OccupationStatus { Kind = periods[0].OccupationKind };

            if (periods.Any(a => a.OccupationKind == OccupationKind.Unavailable))
                return new OccupationStatus { Kind = OccupationKind.Unavailable };

            var values = periods
                .GroupBy(a => a.OccupationKind)
                .Select(a => (a.Key.GetName() + ": " + string.Join(", ", a.OrderBy(s => s.Begin).Select(s => $"{s.Begin.Day}.{s.Begin.Month}-{s.End.Day}.{s.End.Month}"))));

            return new OccupationStatus
            {
                Kind = periods.OrderByDescending(a => a.OccupationKind).First().OccupationKind,
                Value = string.Join(", ", values)
            };
        }
        public virtual OccupationStatus GetStatus(int month)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return GetStatus(firstDayOfMonth, lastDayOfMonth);
        }
    }
}
