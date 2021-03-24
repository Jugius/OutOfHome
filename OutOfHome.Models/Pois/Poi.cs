using System.Linq;
using System.Text;

namespace OutOfHome.Models.Pois
{
    public class Poi : ParsedAddress
	{
        const string UNKNOWN = "unknown";
        public string Description { get; set; }
        public Poi(string formattedAddress, Location location, string provider) : base(string.IsNullOrWhiteSpace(formattedAddress)? UNKNOWN : formattedAddress, location, string.IsNullOrWhiteSpace(provider) ? UNKNOWN : provider) { }
        
        public override string FormattedAddress
        {
            get {
                return ToString();
            }
            set { base.FormattedAddress = value; }
        }
		public override string ToString()
		{
			if(base.FormattedAddress != UNKNOWN)
				return base.FormattedAddress;
			else
			{				
				var sb = new StringBuilder(4);

				//if(!string.IsNullOrWhiteSpace(Country))
				//	sb.Append(Country).Append(", ");

				if(!string.IsNullOrWhiteSpace(City))
					sb.Append(City).Append(", ");

				if(!string.IsNullOrWhiteSpace(Street))
					sb.Append(Street).Append(", ");

				if(!string.IsNullOrWhiteSpace(StreetNumber))
					sb.Append(StreetNumber).Append(", ");

				if(sb.Length > 1)
				{
					sb.Length--;

					string s = sb.ToString();
					if(s.Last() == ',')
						s = s.Remove(s.Length - 1);

					return s;
				}
				else if(Location.IsNullOrEmpty(this.Location))
					return this.Location.ToString();
				else
					return UNKNOWN;
			}
		}
    }
}
