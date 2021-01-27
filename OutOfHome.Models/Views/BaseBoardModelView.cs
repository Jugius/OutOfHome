using OutOfHome.Models.Boards;
using System;

namespace OutOfHome.Models.Views
{
    public abstract class BaseBoardModelView
    {
        public string Supplier { get; set; }
        public string Code { get; set; }
        public string Provider { get; set; }
        public string ProviderID { get; set; }
        public string Side { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetHouse { get; set; }
        public string AddressDescription { get; set; }
        public int? OTS { get; set; }
        public float? GRP { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public int? DoorsDix { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Lighting { get; set; }
        public virtual Uri Photo { get; set; }
        public virtual Uri Map { get; set; }
        public virtual Uri PhotoDoors { get; set; }
        public virtual Uri MapDoors { get; set; }

        public BaseBoardModelView() { }
        public BaseBoardModelView(Board board)
        {
            this.Provider = board.Provider;
            this.ProviderID = board.ProviderID;

            this.Supplier = board.Supplier;
            this.Code = board.SupplierCode;

            this.Region = board.Address.City.Region;
            this.City = board.Address.City.Name;
            this.Street = board.Address.Street;
            this.StreetHouse = board.Address.StreetNumber;
            this.AddressDescription = board.Address.Description;

            if(board.DoorsInfo != null)
            {
                if(board.DoorsInfo.GRP != 0) this.GRP = board.DoorsInfo.GRP;
                if(board.DoorsInfo.OTS != 0) this.OTS = board.DoorsInfo.OTS;
                if(board.DoorsInfo.DoorsID != 0) this.DoorsDix = board.DoorsInfo.DoorsID;
                this.PhotoDoors = board.DoorsInfo.Photo;
                this.MapDoors = board.DoorsInfo.Map; 
            }
            
            this.Latitude = board.Location.Latitude;
            this.Longitude = board.Location.Longitude;

            this.Side = board.Side;
            this.Size = board.Size;
            this.Type = board.Type;
            this.Lighting = board.Lighting;

            this.Photo = board.Photo;
            this.Map = board.Map;            
        }
        public BaseBoardModelView(BaseBoardModelView board)
        {
            this.Provider = board.Provider;
            this.ProviderID = board.ProviderID;

            this.Supplier = board.Supplier;
            this.Code = board.Code;

            this.Region = board.Region;
            this.City = board.City;
            this.Street = board.Street;
            this.StreetHouse = board.StreetHouse;
            this.AddressDescription = board.AddressDescription;

            this.GRP = board.GRP;
            this.OTS = board.OTS;
            this.DoorsDix = board.DoorsDix;

            this.Latitude = board.Latitude;
            this.Longitude = board.Longitude;

            this.Side = board.Side;
            this.Size = board.Size;
            this.Type = board.Type;
            this.Lighting = board.Lighting;

            this.Photo = board.Photo;
            this.PhotoDoors = board.PhotoDoors;
            this.Map = board.Map;
            this.MapDoors = board.MapDoors;
        }       
    }
}
