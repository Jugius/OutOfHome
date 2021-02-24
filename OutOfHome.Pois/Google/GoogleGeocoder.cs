using GoogleApi.Entities.Maps.Geocoding;
using OutOfHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OutOfHome.Pois.Google
{
    public class GoogleGeocoder : IGeocoder
    {
        public string ApiKey { get; set; }
        public GoogleApi.Entities.Common.Enums.Language Language { get; set; } = GoogleApi.Entities.Common.Enums.Language.Russian;        


        public async Task<IEnumerable<GoogleAddress>> GeocodeAsync(string address, CancellationToken cancellationToken = default)
        {
            var request = BuildWebRequest(address);
            return await ProcessRequest(request, cancellationToken).ConfigureAwait(false);

        }
        public async Task<IEnumerable<GoogleAddress>> GeocodeAsync(Location location, CancellationToken cancellationToken = default)
        {
            var request = BuildWebRequest(location);
            return await ProcessRequest(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GoogleAddress>> GeocodeAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            var request = BuildWebRequest(new Location(latitude, longitude));
            return await ProcessRequest(request, cancellationToken).ConfigureAwait(false);
        }
        private static async Task<IEnumerable<GoogleAddress>> ProcessRequest(BaseGeocodeRequest request, CancellationToken cancellationToken)
        {
            var response = request switch
            {
                GoogleApi.Entities.Maps.Geocoding.Address.Request.AddressGeocodeRequest r => await GoogleApi.GoogleMaps.AddressGeocode.QueryAsync(r, cancellationToken).ConfigureAwait(false),
                GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest r => await GoogleApi.GoogleMaps.LocationGeocode.QueryAsync(r, cancellationToken).ConfigureAwait(false),
                _ => throw new Exception("unknown request format")
            };

            var adresses = ProcessResponse(response);
            return adresses;
        }
        private static IEnumerable<GoogleAddress> ProcessResponse(GeocodeResponse response)
        {
            //if(response.Status != GoogleApi.Entities.Common.Enums.Status.Ok && response.Status != GoogleApi.Entities.Common.Enums.Status.ZeroResults)
            //    throw new GoogleGeocodingException(response.Status.GetValueOrDefault());

            if(response.Status == GoogleApi.Entities.Common.Enums.Status.Ok)
                return response.Results.Select(a => GoogleAddress.Build(a));

            return Array.Empty<GoogleAddress>();
        }
        private GoogleApi.Entities.Maps.Geocoding.Address.Request.AddressGeocodeRequest BuildWebRequest(string address)
        {
            if(string.IsNullOrEmpty(this.ApiKey))
                throw new GoogleApi.Exceptions.GoogleApiException("No Api Key");
            return new GoogleApi.Entities.Maps.Geocoding.Address.Request.AddressGeocodeRequest
            {
                Address = address,
                Key = ApiKey,
                Language = this.Language
            };
        }

        private GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest BuildWebRequest(Models.Location location)
        {
            if(string.IsNullOrEmpty(this.ApiKey))
                throw new GoogleApi.Exceptions.GoogleApiException("No Api Key");
            return new GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest
            {
                Location = new GoogleApi.Entities.Common.Location(location.Latitude, location.Longitude),
                Key = ApiKey,
                Language = this.Language
            };
        }
        

        async Task<IEnumerable<Address>> IGeocoder.GeocodeAsync(string address, CancellationToken cancellationToken)
        {
            return await GeocodeAsync(address, cancellationToken).ConfigureAwait(false);
        }

        async Task<IEnumerable<Address>> IGeocoder.GeocodeAsync(Location location, CancellationToken cancellationToken)
        {
            return await GeocodeAsync(location, cancellationToken).ConfigureAwait(false);
        }

        async Task<IEnumerable<Address>> IGeocoder.GeocodeAsync(double latitude, double longitude, CancellationToken cancellationToken)
        {
            return await GeocodeAsync(latitude, longitude, cancellationToken).ConfigureAwait(false);
        }
    }
}
