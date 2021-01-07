using System;
using System.Text.Json.Serialization;

namespace OutOfHome.Models.Boards.SupplierInfo
{
    public class OccupationPeriod
    {
        public DateTimePeriod Period { get; set; }
        public OccupationKind OccupationKind { get; set; }
        [JsonIgnore]
        public string OriginalString { get; set; }
    }
}
