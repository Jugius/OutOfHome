using System;

namespace OutOfHome.Models
{
    public class DoorsInfo
    {
        public int OTS { get; set; }
        public float GRP { get; set; }
        public int DoorsID { get; set; }
        public string InspectionsLamellaIdentifier { get; set; }
        public Uri Photo { get; set; }
        public Uri Map { get; set; }
    }
}
