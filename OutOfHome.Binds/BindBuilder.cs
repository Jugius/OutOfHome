using OutOfHome.Models;
using OutOfHome.Models.Binds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutOfHome.Binds
{
    public class BindBuilder
    {
        public string ApiKey { get; set; }
        public async Task<Bind> BuildBindAsync(string address)
        {
            if(Location.TryParse(address, out Location l))
                return await BuildBindAsync(l, address);

            GoogleBind bind = new GoogleBind { OriginalAddress = address };
            GoogleApi.Entities.Maps.Geocoding.GeocodeResponse response = await GetResponse(address);

            if(response.Status != GoogleApi.Entities.Common.Enums.Status.Ok)
                throw new Exception($"Google Api: {response.Status}");

            var addresses = response.Results.Select(a => GoogleBindAddress.Build(a));
            var basicAddress = GetBasicAddress(addresses);
            bind.Addresses = addresses;

            foreach(var adr in addresses.Where(a => !a.Equals(basicAddress)))
            {
                basicAddress.UpdatePropertiesFrom(adr);
            }
            bind.Address = basicAddress;

            return bind;
        }
        public Task<Bind> BuildBindAsync(Location location)
        {
            return BuildBindAsync(location, null);
        }
        public async Task<Bind> BuildBindAsync(Location location, string originalAddress)
        {
            GoogleBind bind = new GoogleBind 
            {
                OriginalAddress = originalAddress ?? location.ToString() 
            };
            GoogleApi.Entities.Maps.Geocoding.GeocodeResponse response = await GetResponse(location);

            if(response.Status != GoogleApi.Entities.Common.Enums.Status.Ok)
                throw new Exception($"Google Api: {response.Status}");

            var addresses = response.Results.Select(a => GoogleBindAddress.Build(a));
            var basicAddress = GetBasicAddress(addresses, location);
            bind.Addresses = addresses;

            foreach(var adr in addresses.Where(a => !a.Equals(basicAddress)))
            {
                basicAddress.UpdatePropertiesFrom(adr);
            }
            bind.Address = basicAddress;

            return bind;
        }
        public Task<GoogleApi.Entities.Maps.Geocoding.GeocodeResponse> GetResponse(string address)
        {
            var request = new GoogleApi.Entities.Maps.Geocoding.Address.Request.AddressGeocodeRequest
            {
                Address = address,
                Key = ApiKey,
                Language = GoogleApi.Entities.Common.Enums.Language.Russian
            };
            return GoogleApi.GoogleMaps.AddressGeocode.QueryAsync(request);
        }
        public Task<GoogleApi.Entities.Maps.Geocoding.GeocodeResponse> GetResponse(Location location)
        {
            var request = new GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest
            {
                Location = new GoogleApi.Entities.Common.Location(location.Latitude, location.Longitude),
                Key = ApiKey,
                Language = GoogleApi.Entities.Common.Enums.Language.Russian
            };
            return GoogleApi.GoogleMaps.LocationGeocode.QueryAsync(request);
        }

        private static GoogleBindAddress GetBasicAddress(IEnumerable<GoogleBindAddress> addresses)
        {
            var priority = addresses.FirstOrDefault(a =>
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Street_Address && (a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop || a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center)) ||
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Route && a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center) ||
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Premise && a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop));

            return priority ?? addresses.First();
        }
        private static GoogleBindAddress GetBasicAddress(IEnumerable<GoogleBindAddress> addresses, Location location)
        {
            var priority = addresses.Where(a =>
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Street_Address && (a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop || a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center)) ||
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Route && a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Geometric_Center) ||
                (a.Type == GoogleApi.Entities.Common.Enums.PlaceLocationType.Premise && a.LocationType == GoogleApi.Entities.Maps.Geocoding.Common.Enums.GeometryLocationType.Rooftop));

            GoogleBindAddress nearest = null;
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




    }
}
