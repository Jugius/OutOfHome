using OutOfHome.Models.Pois;
using System;

namespace OutOfHome.Exports
{
    public class PoiPropertyGetter : PropertyGetter<Poi>
    {       
        public PoiProperty Kind { get; set; }        
        public PoiPropertyGetter(PoiProperty kind)
        {
            this.Kind = kind;            
        }
        public PoiPropertyGetter() { }
        public override object GetPropertyValueFrom(Poi poi)
        {
            if(poi == null) throw new ArgumentNullException(nameof(poi));

            return this.Kind switch
            {
                PoiProperty.Provider => poi.Provider,                
                PoiProperty.Description => poi.Description,
                PoiProperty.Country => poi.Country,
                PoiProperty.Region => poi.Region,
                PoiProperty.City => poi.City,
                PoiProperty.District => poi.District,
                PoiProperty.Street => poi.Street,
                PoiProperty.StreetNumber => poi.StreetNumber,
                PoiProperty.FormattedAddress => poi.FormattedAddress,
                PoiProperty.Location => poi.Location.ToString(),
                PoiProperty.Latitude => poi.Location.Latitude,
                PoiProperty.Longitude => poi.Location.Longitude,
                PoiProperty.URL_Map => new Uri(@"https://www.google.com/maps/search/" + poi.Location.ToString()),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }
        public object GetPropertyValueFrom(Models.Views.BasePoiModelView poi)
        {
            if(poi == null) throw new ArgumentNullException(nameof(poi));

            return this.Kind switch
            {
                PoiProperty.Provider => poi.Provider,
                PoiProperty.OriginalAddress => poi.SourceQueryString,
                PoiProperty.Description => poi.Description,
                PoiProperty.Country => poi.Country,
                PoiProperty.Region => poi.Region,
                PoiProperty.City => poi.City,
                PoiProperty.District => poi.District,
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
            return source switch
            { 
                Models.Views.BasePoiModelView p => GetPropertyValueFrom(p),
                Models.Pois.Poi p => GetPropertyValueFrom(p),
                _ => throw new Exception($"Неизвестный тип {source.GetType()}")
            };
        }

        
    }
}
