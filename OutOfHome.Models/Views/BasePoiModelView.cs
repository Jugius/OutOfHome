namespace OutOfHome.Models.Views
{
    public class BasePoiModelView
    {
        //Address properties
        public string Address { get; set; }
        public Location Location { get; set; }
        public string Provider { get; set; }

        //Parsed Address properties        
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        //Additional
        public string Description { get; set; }
        public string SourceQueryStrign { get; set; }
        public BasePoiModelView() { }
        public BasePoiModelView(Pois.Poi poi)
        {
            this.Address = poi.FormattedAddress;
            this.Location = poi.Location;
            this.Provider = poi.Provider;

            this.Country = poi.Country;
            this.Region = poi.Region;
            this.City = poi.City;
            this.District = poi.District;
            this.Zip = poi.Zip;
            this.Street = poi.Street;
            this.StreetNumber = poi.StreetNumber;

            this.Description = poi.Description;
        }
        public BasePoiModelView(BasePoiModelView view)
        {
            this.Address = view.Address;
            this.Location = view.Location;
            this.Provider = view.Provider;

            this.Country = view.Country;
            this.Region = view.Region;
            this.City = view.City;
            this.District = view.District;
            this.Zip = view.Zip;
            this.Street = view.Street;
            this.StreetNumber = view.StreetNumber;

            this.Description = view.Description;
            this.SourceQueryStrign = view.SourceQueryStrign;
        }
    }

}
