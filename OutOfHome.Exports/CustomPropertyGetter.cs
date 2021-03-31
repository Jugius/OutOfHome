using System.Text.RegularExpressions;

namespace OutOfHome.Exports
{
    public class CustomPropertyGetter : ICustomPropertyGetter
    {
        private static readonly Regex AspectRegex = new Regex(Regex.Escape("{") + "([^{}]*)" + Regex.Escape("}"));
        public virtual string CustomProperty { get; set; }
        public object GetPropertyValueFrom(object source)
        {
            if(string.IsNullOrEmpty(this.CustomProperty))
                return null;
            
            MatchCollection maches = AspectRegex.Matches(this.CustomProperty);
            
            if(maches.Count == 0) return GetPropertyValueByName(source, this.CustomProperty);
            if(maches.Count == 1) return GetPropertyValueByName(source, maches[0].Groups[1].Value);

            string customProperty = this.CustomProperty;
            foreach(Match m in maches)
            {
                var result = GetPropertyValueByName(source, m.Groups[1].Value);
                if(result == null) return customProperty;
                customProperty = customProperty.Replace(m.Value, result.ToString());
            }
            return customProperty;
        }
        private static object GetPropertyValueByName(object obj, string propertyName)
        {
            object result = obj;
            foreach(var part in propertyName.Split('.'))
            {
                if(result == null) return null;

                var type = result.GetType();
                var info = type.GetProperty(part);
                if(info == null) return null;

                result = info.GetValue(result, null);
            }
            return result;
        }
    }
}
