using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models
{
    /// <summary>
	/// Generic parsed address with each field separated out form the original FormattedAddress
	/// </summary>
	public class ParsedAddress : Address
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public ParsedAddress(string formattedAddress, Location coordinates, string provider)
            : base(formattedAddress, coordinates, provider) { }
    }
}
