
using System.Text.RegularExpressions;

namespace OutOfHome.Exports
{
    public abstract class PropertyGetter<TSource> : IPropertyGetter where TSource : class
    {
        
        public abstract object GetPropertyValueFrom(TSource source);
        public virtual object GetPropertyValueFrom(object source) =>  GetPropertyValueFrom(source as TSource);    
    }
}
