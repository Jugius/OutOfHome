using System;
using System.Collections.Generic;
using System.Text;

namespace OutOfHome.Models.Binds
{
    public enum BindProperty
    {
        Provider = 101,
        ProviderPlaceId = 102,

        OriginalAddress = 201,
        Description = 202,

        Country = 301,
        Region = 302,
        City = 303,
        District = 304,
        Zip = 305,
        Street = 306,
        StreetNumber = 307,
        FormattedAddress = 308,

        Intersection = 401,

        Location = 501,
        Latitude = 502,
        Longitude = 503,

        URL_Map = 601
    }
}
