using System;
using System.Collections.Generic;

namespace OutOfHome.Models
{
    public enum OccupationKind
    {
        Free = 1,
        Reserved = 2,
        Booked = 3,
        Unavailable = 4,
        Unrecognized = 5
    }
    public static class OccKindExt
    {
        private static readonly Dictionary<OccupationKind, string> OccupationStatus = new Dictionary<OccupationKind, string>
        {
            { OccupationKind.Booked, "Бронь" },
            { OccupationKind.Reserved, "Резерв" },
            { OccupationKind.Unavailable, "Неактив" },
            { OccupationKind.Free, "" },
            { OccupationKind.Unrecognized, "Неизв"}
        };
        public static string GetName(this OccupationKind kind) => OccupationStatus[kind];
    }
}
