using OutOfHome.Models;
using OutOfHome.Models.Pois;
using OutOfHome.Pois.Google;
using OutOfHome.Pois.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutOfHome.Pois
{
    public class PoiBuilder
    {
        public MapProvider Provider { get; }
        public string ApiKey { get; set; }
        public PoiBuilder(MapProvider provider) { this.Provider = provider; }
        public async Task<PoiGeocodeResult> GeocodePoi(string address)
        {
            PoiGeocodeResult result = new PoiGeocodeResult { QueryString = address };
            if(Location.TryParse(address, out Location loc))
            {                
                var locResult = await GeocodePoi(loc);
                result.Addresses = locResult.Addresses;
                result.Poi = locResult.Poi;
                result.Error = locResult.Error;
            }
            else
            {
                Google.GoogleGeocoder coder = GetGeocoder();
                try
                {
                    var addresses = await coder.GeocodeAsync(address);

                    if(!addresses.Any())
                    {
                        result.Error = new Exception("Адрес не найден");
                        return result;
                    }

                    var basicAddress = GetBasicAddress(addresses);
                    Poi poi = BuildPoi(basicAddress);

                    foreach(var adr in addresses.Where(a => !a.Equals(basicAddress)))
                    {
                        poi.UpdatePropertiesFrom(adr);
                    }
                    result.Addresses = addresses;
                    result.Poi = poi;
                }
                catch(Exception ex)
                {
                    result.Error = ex;
                }                
            }
            return result;
        }
        public async Task<PoiGeocodeResult> GeocodePoi(Location location)
        {
            PoiGeocodeResult result = new PoiGeocodeResult { QueryString = location.ToString() };
            Google.GoogleGeocoder coder = GetGeocoder();
            try
            {
                var addresses = await coder.GeocodeAsync(location);

                if(!addresses.Any())
                {
                    result.Error = new Exception("Адрес не найден");
                    result.Poi = new Poi(null, location, "Google");
                    return result;
                }

                var basicAddress = GetBasicAddress(addresses, location);
                Poi poi = BuildPoi(basicAddress);
                poi.Location = location;

                foreach(var adr in addresses.Where(a => !a.Equals(basicAddress)))
                {
                    poi.UpdatePropertiesFrom(adr);
                }
                result.Addresses = addresses;
                result.Poi = poi;
            }
            catch(Exception ex)
            {
                result.Poi = new Poi(null, location, "Google");
                result.Error = ex;
            }
            return result;
        }
        private Google.GoogleGeocoder GetGeocoder() => this.Provider switch 
        {
             MapProvider.Google => new Google.GoogleGeocoder { ApiKey = this.ApiKey, Language = GoogleApi.Entities.Common.Enums.Language.Russian },
             _ => throw new Exception($"NotImplemented provider: {this.Provider}")
        };

        private static GoogleAddress GetBasicAddress(IEnumerable<GoogleAddress> addresses)
        {
            var priority = addresses.FirstOrDefault(a =>
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Street_Address && (a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop || a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center)) ||
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Route && a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center) ||
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Premise && a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop));

            return priority ?? addresses.First();
        }
        private static GoogleAddress GetBasicAddress(IEnumerable<GoogleAddress> addresses, Location location)
        {
            var priority = addresses.Where(a =>
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Street_Address && (a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop || a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center)) ||
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Route && a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center) ||
                (a.PlaceType == GoogleApi.Entities.Common.Enums.PlaceLocationType.Premise && a.GeometryType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop));

            GoogleAddress nearest = null;
            if(priority.Any())
            {
                double nearestDistance = double.MaxValue;
                foreach(var address in priority)
                {
                    var distance = address.Location.DistanceBetween(location).Value;
                    if(distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearest = address;
                    }
                }
            }
            else nearest = addresses.First();
            nearest.Location = location;
            return nearest;
        }
        private static Poi BuildPoi(ParsedAddress address) => new Poi(null, address.Location, address.Provider)
        {
            City = address.City,
            Country = address.Country,
            District = address.District,
            Region = address.Region,
            Street = address.Street,
            StreetNumber = address.StreetNumber,
            Zip = address.Zip
        };
    }
    internal static class Ext 
    {
        public static void UpdatePropertiesFrom(this ParsedAddress thisAddress, ParsedAddress other)
        {
            bool streetchanged = false;
            if(string.IsNullOrEmpty(thisAddress.Country) && !string.IsNullOrEmpty(other.Country)) thisAddress.Country = other.Country;
            if(string.IsNullOrEmpty(thisAddress.Region) && !string.IsNullOrEmpty(other.Region)) thisAddress.Region = other.Region;
            if(string.IsNullOrEmpty(thisAddress.City) && !string.IsNullOrEmpty(other.City)) thisAddress.City = other.City;
            if(string.IsNullOrEmpty(thisAddress.Zip) && !string.IsNullOrEmpty(other.Zip)) thisAddress.Zip = other.Zip;
            if(string.IsNullOrEmpty(thisAddress.Street) && !string.IsNullOrEmpty(other.Street))
            {
                thisAddress.Street = other.Street;
                streetchanged = true;
            }
            if(string.IsNullOrEmpty(thisAddress.StreetNumber) && !string.IsNullOrEmpty(other.StreetNumber) && streetchanged) thisAddress.StreetNumber = other.StreetNumber;
            if(string.IsNullOrEmpty(thisAddress.District) && !string.IsNullOrEmpty(other.District)) thisAddress.District = other.District;
        }
    }
}
