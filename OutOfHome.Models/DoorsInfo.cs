using System;

namespace OutOfHome.Models
{
    public class DoorsInfo
    {
        public int OTS { get; set; }
        public float GRP { get; set; }
        public int DoorsID { get; set; }
        /// <summary>
        /// NUMBER_INS (номер ламели для инспекций)
        /// </summary>
        public string NumberIns { get; set; }
        public Uri Photo { get; set; }
        public Uri Map { get; set; }
        /// <summary>
        /// D_ID
        /// </summary>
        public int ConstructionID { get; set; }
    }
}
