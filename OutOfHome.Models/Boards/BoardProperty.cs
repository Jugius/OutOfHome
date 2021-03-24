namespace OutOfHome.Models.Boards
{
    public enum BoardProperty
    {
        ProviderID = 101,
        Provider = 102,

        Supplier = 201,
        SupplierCode = 202,

        Region = 301,
        City = 302,
        Street = 303,
        StreetNumber = 304,
        Address = 306,
        AddressDescription = 305,

        Side = 401,
        Kind = 402,
        Size = 403,
        Light = 404,
        Location = 405,
        Angle = 406,

        URL_Photo = 501,
        URL_Map = 502,
        URL_DoorsPhoto = 503,
        URL_DoorsMap = 504,
        URL_StreetsView = 505,
        URL_GoogleMapPoint = 506,

        DoorsId = 601,
        OTS = 602,
        GRP = 603,

        Price = 701,
        OccSource = 702,

        Custom = 801,

        Color = 901,

        BindLocation = 1001,
        BindAddress = 1002,
        BindDescription = 1003,
        BindDistance = 1004,
        BindMap = 1005
    }
}
