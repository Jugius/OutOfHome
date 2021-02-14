using System;

namespace OutOfHome.Models
{
    public class City : IEquatable<City>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public int Population { get; set; }
        public Location Center { get; set; }       
        public bool IsRegionalCenter { get; set; }
        public bool IsCapital { get; set; }
        public int? DoorsId { get; set; }
        public string DoorsShortName { get; set; }
        public int? BmaId { get; set; }
        public string OctagonId { get; set; }

        public override bool Equals(object other)
        {
            if(other == null) return false;

            if(object.ReferenceEquals(this, other)) return true;

            return this.Equals(other as City);
        }
        public bool Equals(City other)
        {
            if(other == null) return false;

            return this.Name == other.Name &&
                    this.Region == other.Region;
        }
        public override string ToString()
        {
            return this.Name;
        }
        public override int GetHashCode() => (this.Name + this.Region).GetHashCode();        
    }
}
