using System;

namespace OutOfHome.Models.Boards
{
    public class Board
    {
        public string ProviderID { get; set; }
        public string Supplier { get; set; }
        public string SupplierCode { get; set; }
        public BoardAddress Address { get; set; }
        public Location Location { get; set; }
        public string Side { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public bool Lighting { get; set; }
        public int? Angle { get; set; }
        public DoorsInfo DoorsInfo { get; set; }
        public Uri Photo { get; set; }
        public Uri Map { get; set; }
        public string Provider { get; set; }

        public virtual int GetSideHashCode()
        {
            string s = this.Location.ToString() + this.Side;
            if(this.Angle.HasValue)
                s += this.Angle.ToString();
            return s.GetHashCode();
        }
    }
}
