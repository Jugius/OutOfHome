namespace OutOfHome.Models.Binds
{
    public class BindAddress
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Intersection { get; set; }
        public string PlaceId { get; set; }
        public Location Location { get; set; }
        public virtual string FormattedAddress 
        { 
            get => _formattedAddress ?? this.ToString();
            set => _formattedAddress = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _formattedAddress;
        public override string ToString()
        {
            return $"{City}, {Street}, {StreetNumber}, {Intersection}".TrimEnd(',', ' ');
        }
        public virtual void UpdatePropertiesFrom(BindAddress other)
        {
            bool streetchanged = false;
            if(string.IsNullOrEmpty(this.Country) && !string.IsNullOrEmpty(other.Country)) this.Country = other.Country;
            if(string.IsNullOrEmpty(this.Region) && !string.IsNullOrEmpty(other.Region)) this.Region = other.Region;
            if(string.IsNullOrEmpty(this.City) && !string.IsNullOrEmpty(other.City)) this.City = other.City;
            if(string.IsNullOrEmpty(this.Zip) && !string.IsNullOrEmpty(other.Zip)) this.Zip = other.Zip;
            if(string.IsNullOrEmpty(this.Street) && !string.IsNullOrEmpty(other.Street))
            {
                this.Street = other.Street;
                streetchanged = true;
            }
            if(string.IsNullOrEmpty(this.StreetNumber) && !string.IsNullOrEmpty(other.StreetNumber) && streetchanged) this.StreetNumber = other.StreetNumber;
            if(string.IsNullOrEmpty(this.District) && !string.IsNullOrEmpty(other.District)) this.District = other.District;
            if(string.IsNullOrEmpty(this.Intersection) && !string.IsNullOrEmpty(other.Intersection)) this.Intersection = other.Intersection;
        }
    }
}
