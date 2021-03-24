using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Maps.Geocoding.Common.Enums;
using Newtonsoft.Json;
using System.Linq;

namespace OutOfHome.Pois.Google
{
    public class GoogleAddress : Models.ParsedAddress
    {
        const string ProviderGoogle = "Google";
        public PlaceLocationType PlaceType { get; private set; }
        public GeometryLocationType GeometryType { get; private set; }
        public string Intersection { get; set; }
        public string PlaceId { get; set; }
        public GoogleAddress(string formattedAddress, Models.Location location) : base(formattedAddress, location, ProviderGoogle) { }
        public static GoogleAddress Build(GoogleApi.Entities.Maps.Geocoding.Common.Result result)
        {
            //string addressShortName = string.Empty;
            string addressCountry = string.Empty;
            string addressAdministrativeAreaLevel1 = string.Empty;
            //string addressAdministrativeAreaLevel2 = string.Empty;
            //string addressAdministrativeAreaLevel3 = string.Empty;
            //string addressColloquialArea = string.Empty;
            string addressLocality = string.Empty;
            //string addressSublocality = string.Empty;
            //string addressNeighborhood = string.Empty;
            string addressStreet = string.Empty;
            string addressStreetNumber = string.Empty;
            string addressPostalCode = string.Empty;
            string addressDistrict = string.Empty;
            string addressIntersection = string.Empty;

            foreach(var c in result.AddressComponents)
            {
                switch(c.Types.First())
                {
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Route:
                        if(c.LongName != "Unnamed Road") addressStreet = c.LongName;
                        break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Intersection:
                        addressIntersection = c.LongName;
                        break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Political:
                        addressDistrict = c.ShortName;
                        break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Country:
                        addressCountry = c.LongName;
                        break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Administrative_Area_Level_1:
                        addressAdministrativeAreaLevel1 = c.ShortName;
                        break;
                    //case GoogleApi.Entities.Common.Enums.AddressComponentType.Colloquial_Area:
                    //    addressColloquialArea = c.LongName;
                    //break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Locality:
                        addressLocality = c.LongName;
                        break;
                    //case GoogleApi.Entities.Common.Enums.AddressComponentType.Sublocality:
                    //    addressSublocality = c.LongName;
                    //    break;
                    //case GoogleApi.Entities.Common.Enums.AddressComponentType.Neighborhood:
                    //    addressNeighborhood = c.LongName;
                    //    break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Street_Number:
                        addressStreetNumber = c.ShortName;
                        break;
                    case GoogleApi.Entities.Common.Enums.AddressComponentType.Postal_Code:
                        addressPostalCode = c.LongName;
                        break;

                    default:
                        break;

                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Uknown:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Street_Address:                    
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Administrative_Area_Level_2:                        
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Administrative_Area_Level_3:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Administrative_Area_Level_4:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Administrative_Area_Level_5:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Ward:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Sublocality_Level_1:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.SublocalityLevel2:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Sublocality_Level_3:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Sublocality_Level_4:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Sublocality_Level_5:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Premise:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Subpremise:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Natural_Feature:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Airport:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Park:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Floor:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Establishment:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Point_Of_Interest:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Parking:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Post_Box:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Postal_Town:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Room:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Bus_Station:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Train_Station:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Transit_Station:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Geocode:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Postal_Code_Prefix:
                        //case GoogleApi.Entities.Common.Enums.AddressComponentType.Postal_Code_Suffix:                   
                }
            }

            return new GoogleAddress(result.FormattedAddress, new Models.Location(result.Geometry.Location.Latitude, result.Geometry.Location.Longitude))
            {
                Country = addressCountry,
                Region = addressAdministrativeAreaLevel1,
                City = addressLocality,
                Zip = addressPostalCode,
                Street = addressStreet,
                StreetNumber = addressStreetNumber,
                District = addressDistrict,
                Intersection = addressIntersection,
                PlaceId = result.PlaceId,
                PlaceType = result.Types.First(),
                GeometryType = result.Geometry.LocationType
            };
        }
    }
}
