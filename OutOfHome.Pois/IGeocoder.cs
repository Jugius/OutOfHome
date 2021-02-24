using OutOfHome.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OutOfHome.Pois
{
    public interface IGeocoder
    {
        Task<IEnumerable<Address>> GeocodeAsync(string address, CancellationToken cancellationToken = default(CancellationToken));
        //Task<IEnumerable<Poi>> GeocodeAsync(string street, string city, string state, string postalCode, string country, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Address>> GeocodeAsync(Location location, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<Address>> GeocodeAsync(double latitude, double longitude, CancellationToken cancellationToken = default(CancellationToken));
    }
}
