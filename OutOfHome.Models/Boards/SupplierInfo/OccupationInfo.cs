using System;
using System.Collections.Generic;
using System.Linq;

namespace OutOfHome.Models.Boards.SupplierInfo
{
    public class OccupationInfo
    {
        public List<OccupationPeriod> OccupationPeriods { get; } = new List<OccupationPeriod>();
        public DateTimePeriod? VisiblePeriod { get; set; }
        public string OriginOccupationString { get; set; }
        public virtual OccupationStatus GetStatus(DateTime start, DateTime end)
        {
            
            if(start > this.VisiblePeriod?.End || end < this.VisiblePeriod?.Start)
                return new OccupationStatus(OccupationKind.OutOfRange);

            if (OccupationPeriods.Count == 0) 
                return new OccupationStatus(OccupationKind.Free);

            var periods = OccupationPeriods.Where(a => !(a.Period.Start > end || a.Period.End < start)).ToList();


            if (periods.Count == 0)
                return new OccupationStatus(OccupationKind.Free);

            if(periods.Count == 1)
                return new OccupationStatus(periods.First().OccupationKind);

            if(periods.Any(a => a.OccupationKind == OccupationKind.Unavailable))
                return new OccupationStatus(OccupationKind.Unavailable);

            var values = periods
                .GroupBy(a => a.OccupationKind)
                .Select(a => (a.Key.GetName() + ": " + string.Join(", ", a.OrderBy(s => s.Period.Start).Select(s => (s.Period.Start.ToString("dd.MM") + '-' + s.Period.End.ToString("dd.MM"))))));

            return new OccupationStatus(periods.OrderByDescending(a => a.OccupationKind).First().OccupationKind)
            {                
                Value = string.Join(", ", values)
            };
        }
        public virtual OccupationKind GetSingleStatus(DateTime start, DateTime end) 
        {
            if(start > this.VisiblePeriod?.End || end < this.VisiblePeriod?.Start)
                return OccupationKind.OutOfRange;

            if(OccupationPeriods == null || OccupationPeriods.Count == 0)
                return OccupationKind.Free;

            var periods = OccupationPeriods.Where(a => !(a.Period.Start > end || a.Period.End < start)).OrderByDescending(a => a.OccupationKind);

            return periods.Any() ? periods.First().OccupationKind : OccupationKind.Free;
        }
    }
}
