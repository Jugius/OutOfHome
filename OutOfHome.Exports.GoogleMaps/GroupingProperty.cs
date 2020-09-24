using OutOfHome.Models.Views;
using System;

namespace OutOfHome.Exports.GoogleMaps
{
    public enum GroupingProperty
    {
        None,
        City,
        Supplier,
        Size,
        Kind,
        Description
    }
    public static class GroupingPropertyExtention
    {
        public static Func<BaseBoardModelView, string> GetGroupSelector(this GroupingProperty property)
        {
            return property switch
            {
                GroupingProperty.None => null,
                GroupingProperty.City => new Func<BaseBoardModelView, string>(a => a.City),
                GroupingProperty.Supplier => new Func<BaseBoardModelView, string>(a => a.Supplier),
                GroupingProperty.Size => new Func<BaseBoardModelView, string>(a => a.Size),
                GroupingProperty.Kind => new Func<BaseBoardModelView, string>(a => a.Type),
                _ => throw new Exception("Create of selector has not implemented for property: " + property.ToString()),
            };
        }
    }
}
