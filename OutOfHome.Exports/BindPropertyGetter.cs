using OutOfHome.Models.Binds;
using System;
using System.Collections.Generic;

namespace OutOfHome.Exports
{
    public class BindPropertyGetter : PropertyGetter
    {
        private static readonly HashSet<BindProperty> PropertiesWithHyperlinks = new HashSet<BindProperty>
        {
            BindProperty.URL_Map
        };
        public BindProperty Kind { get; set; }
        public string Name 
        {
            get => _name ?? DefaultNames[this.Kind];
            set => _name = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _name;
        public virtual string NumberFormat => this.Kind switch
        {
            BindProperty.Latitude => "0.00000###",
            BindProperty.Longitude => "0.00000###",
            _ => null,
        };
        public bool IsHyperlink { get; set; } = false;
        public BindPropertyGetter(BindProperty kind)
        {
            this.Kind = kind;
            this.IsHyperlink = PropertiesWithHyperlinks.Contains(kind);
        }
        public BindPropertyGetter() { }

        public object GetPropertyValueFrom(Bind bind)
        {
            return this.Kind switch
            {
                BindProperty.Provider => throw new NotImplementedException(),
                BindProperty.ProviderPlaceId => bind.Address?.PlaceId,
                BindProperty.OriginalAddress => bind.OriginalAddress,
                BindProperty.Description => bind.Description,
                BindProperty.Country => bind.Address?.Country,
                BindProperty.Region => bind.Address?.Region,
                BindProperty.City => bind.Address?.City,
                BindProperty.District => bind.Address?.District,
                BindProperty.Zip => bind.Address?.Zip,
                BindProperty.Street => bind.Address?.Street,
                BindProperty.StreetNumber => bind.Address?.StreetNumber,
                BindProperty.FormattedAddress => bind.Address?.ToString(),
                BindProperty.Intersection => bind.Address?.Intersection,
                BindProperty.Location => bind.Address?.Location.ToString(),
                BindProperty.Latitude => bind.Address?.Location.Latitude,
                BindProperty.Longitude => bind.Address?.Location.Longitude,
                BindProperty.URL_Map => bind.Address == null? null : new Uri(@"https://www.google.com/maps/search/" + bind.Address.Location.ToString()),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }

        public override object GetPropertyValueFrom(object source)
        {
            if(source == null) throw new ArgumentNullException();
            return GetPropertyValueFrom(source as Bind);
        }

        static readonly Dictionary<BindProperty, string> DefaultNames = new Dictionary<BindProperty, string>()
        {
            { BindProperty.Provider, "Provider" },
            { BindProperty.ProviderPlaceId, "Provider ID" },
            
            { BindProperty.OriginalAddress, "Оригинальный запрос" },
            { BindProperty.Description, "Описание" },

            { BindProperty.Country, "Страна" },
            { BindProperty.Region, "Область" },
            { BindProperty.City, "Город" },
            { BindProperty.District, "Район" },
            { BindProperty.Street, "Улица" },
            { BindProperty.StreetNumber, "Дом" },
            { BindProperty.Zip, "Индекс" },
            { BindProperty.FormattedAddress, "Адрес" },

            { BindProperty.Intersection, "Пересечение" },
            
            { BindProperty.Location, "Location" },
            { BindProperty.Latitude, "Lat" },
            { BindProperty.Longitude, "Lon" },
                        
            { BindProperty.URL_Map, "Карта" }
        };
    }
}
