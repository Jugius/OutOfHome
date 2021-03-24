using OutOfHome.Models;
using System;
using System.Collections.Generic;

namespace OutOfHome.Pois.Results
{
    public class PoiGeocodeResult
    {
        public OutOfHome.Models.Pois.Poi Poi { get; set; }
        public Exception Error { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public string QueryString { get; set; }
    }
}
