using OutOfHome.Models.Pois;
using System;
using System.Collections.Generic;

namespace OutOfHome.Exports
{
    public class PoiPropertyGetter : PropertyGetter
    {       
        public PoiProperty Kind { get; set; }
        public string Name 
        {
            get => _name ?? DefaultNames[this.Kind];
            set => _name = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _name;
        public virtual string NumberFormat => this.Kind switch
        {
            PoiProperty.Latitude => "0.00000###",
            PoiProperty.Longitude => "0.00000###",
            _ => null,
        };
        
        public PoiPropertyGetter(PoiProperty kind)
        {
            this.Kind = kind;            
        }
        public PoiPropertyGetter() { }

        public object GetPropertyValueFrom(Models.Views.BasePoiModelView poi)
        {
            return this.Kind switch
            {
                PoiProperty.Provider => poi.Provider,
                //PoiProperty.ProviderPlaceId => //poi. Address?.PlaceId,
                PoiProperty.OriginalAddress => poi.SourceQueryStrign,
                PoiProperty.Description => poi.Description,
                PoiProperty.Country => poi.Country,
                PoiProperty.Region => poi.Region,
                PoiProperty.City => poi.City,
                PoiProperty.District => poi.District,
                //PoiProperty.Zip => poi.Zip,
                PoiProperty.Street => poi.Street,
                PoiProperty.StreetNumber => poi.StreetNumber,
                PoiProperty.FormattedAddress => poi.Address,
                PoiProperty.Location => poi.Location.ToString(),
                PoiProperty.Latitude => poi.Location.Latitude,
                PoiProperty.Longitude => poi.Location.Longitude,
                PoiProperty.URL_Map => new Uri(@"https://www.google.com/maps/search/" + poi.Location.ToString()),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }

        public override object GetPropertyValueFrom(object source)
        {
            if(source == null) throw new ArgumentNullException();
            return GetPropertyValueFrom(source as Poi);
        }

        static readonly Dictionary<PoiProperty, string> DefaultNames = new Dictionary<PoiProperty, string>()
        {
            { PoiProperty.Provider, "Provider" },
            //{ PoiProperty.ProviderPlaceId, "Provider ID" },
            
            { PoiProperty.OriginalAddress, "Оригинальный запрос" },
            { PoiProperty.Description, "Описание" },

            { PoiProperty.Country, "Страна" },
            { PoiProperty.Region, "Область" },
            { PoiProperty.City, "Город" },
            { PoiProperty.District, "Район" },
            { PoiProperty.Street, "Улица" },
            { PoiProperty.StreetNumber, "Дом" },
            //{ PoiProperty.Zip, "Индекс" },
            { PoiProperty.FormattedAddress, "Адрес" },
          
            { PoiProperty.Location, "Location" },
            { PoiProperty.Latitude, "Lat" },
            { PoiProperty.Longitude, "Lon" },
                        
            { PoiProperty.URL_Map, "Карта" }
        };
    }
}
