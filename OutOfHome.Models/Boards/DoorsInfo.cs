using System;

namespace OutOfHome.Models.Boards
{
    public class DoorsInfo
    {
        /// <summary>
        /// Ots
        /// </summary>
        public int OTS { get; set; }
        /// <summary>
        /// Grp
        /// </summary>
        public float GRP { get; set; }
        /// <summary>
        /// Dix
        /// </summary>
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
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDay { get; set; }
    }
}
