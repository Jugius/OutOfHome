namespace OutOfHome.Models
{
    public class BoardAddress
    {
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Description { get; set; }
        public string FormattedAddress
        {
            get
            {
                string outStr = Street;
                if (!string.IsNullOrEmpty(StreetNumber))
                    outStr += ", " + StreetNumber;
                if (!string.IsNullOrEmpty(Description))
                    outStr += ", " + Description;
                return outStr;
            }
        }
    }
}
