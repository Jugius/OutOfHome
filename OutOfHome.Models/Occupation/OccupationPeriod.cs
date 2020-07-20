using System;

namespace OutOfHome.Models.Occupation
{
    public class OccupationPeriod
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public OccupationKind OccupationKind { get; set; }
        public string OriginalString { get; set; }
    }
}
