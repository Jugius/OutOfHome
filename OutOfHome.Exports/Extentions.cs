using System;
using System.Collections.Generic;
using System.Text;

namespace OutOfHome.Exports
{
    internal static class Extentions
    {
        public static object GetPropertyValueByName(this object obj, string propertyName)
        {
            object result = obj;
            foreach (var part in propertyName.Split('.'))
            {
                if (result == null) { return null; }

                var type = result.GetType();
                var info = type.GetProperty(part);
                if (info == null) { return null; }

                result = info.GetValue(result, null);
            }
            return result;
        }
    }
}
