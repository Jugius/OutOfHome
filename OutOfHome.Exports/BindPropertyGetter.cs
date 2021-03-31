using OutOfHome.Models;
using OutOfHome.Models.Binds;
using OutOfHome.Models.Views;
using System;


namespace OutOfHome.Exports
{
    public class BindPropertyGetter : PropertyGetter<Bind>
    {
        private const string BindGoogleMap = @"https://www.google.com/maps/dir/";
        public BindProperty Kind { get; set; }
        public BindPropertyGetter(BindProperty property)
        {
            this.Kind = property;
        }
        public override object GetPropertyValueFrom(Bind bind)
        {
            return this.Kind switch
            {
                BindProperty.Distance => bind.Board.Location.DistanceBetween(bind.Poi.Location, Models.DistanceUnit.Kilometers).Value * 1000,
                BindProperty.Map => new Uri(BindGoogleMap + bind.Board.Location.ToString() + @"/" + bind.Poi.Location.ToString()),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }
        public object GetPropertyValueFrom(BaseBindModelView bind)
        {
            return this.Kind switch
            {
                BindProperty.Distance => new Location(bind.Board.Latitude, bind.Board.Longitude).DistanceBetween(bind.Poi.Location, Models.DistanceUnit.Kilometers).Value * 1000,
                BindProperty.Map => new Uri(BindGoogleMap + new Location(bind.Board.Latitude, bind.Board.Longitude).ToString() + @"/" + bind.Poi.Location.ToString()),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }
        public override object GetPropertyValueFrom(object source) 
        {
            if(source == null) throw new ArgumentNullException(nameof(source));
            return source switch
            {
                Bind b => GetPropertyValueFrom(b),
                BaseBindModelView mv => GetPropertyValueFrom(mv),
                _ => throw new Exception($"Неизвестный формат Bind {source.GetType()}")
            };            
        }
        
    }
}
