using OfficeOpenXml;
using OutOfHome.Models;
using OutOfHome.Models.Pois;
using System;
using System.Collections.Generic;

namespace OutOfHome.Imports
{
    public class PoiPropertySetter : PropertySetter
    {
        public PoiProperty Kind { get; set; }
        private bool Required
        {
            get {
                switch(this.Kind)
                {
                    case PoiProperty.City:
                    case PoiProperty.Street:
                    case PoiProperty.Latitude:
                    case PoiProperty.Longitude:
                        return true;
                    default: return false;

                }
            }
        }
        public PoiPropertySetter(PoiProperty kind)
        {
            this.Kind = kind;
        }
        public void SetValue(Poi poi, ExcelRange cell)
        {
            if(cell == null)
            {
                if(Required)
                    throw new Exception($"Нулевая ячейка для столбца {this.Kind}");
                else
                    return;
            }

            string val = cell.GetValue<string>();
            if(string.IsNullOrEmpty(val) && this.Required)
                throw new Exception($"Недопустимо пустое значение ячейки для столбца {GetPoiPropertyName(this.Kind)}. Адрес ячейки {cell.Address}");

            switch(this.Kind)
            {
                case PoiProperty.Provider:
                    break;
                //case PoiProperty.ProviderPlaceId:
                //    poi.Address.PlaceId = val;
                //    break;
                //case PoiProperty.OriginalAddress:
                //    poi.OriginalAddress = val;
                //    break;
                case PoiProperty.Description:
                    poi.Description = val;
                    break;
                case PoiProperty.Country:
                    poi.Country = val;
                    break;
                case PoiProperty.Region:
                    poi.Region = val;
                    break;
                case PoiProperty.City:
                    poi.City = val;
                    break;
                case PoiProperty.District:
                    poi.District = val;
                    break;
                //case PoiProperty.Zip:
                //    poi.Zip = val;
                //    break;
                case PoiProperty.Street:
                    poi.Street = val;
                    break;
                case PoiProperty.StreetNumber:
                    poi.StreetNumber = val;
                    break;
                case PoiProperty.FormattedAddress:
                    //bind.Address.FormattedAddress = val;
                    break;
                case PoiProperty.Location:
                    if(OutOfHome.Models.Location.TryParse(val, out Location l))
                        poi.Location = l;
                    else throw new Exception($"Ошибка чтения координат в ячейке {cell.Address}. Значение: {val}");
                    break;
                case PoiProperty.Latitude:
                    poi.Location.Latitude = cell.GetValue<double>();
                    break;
                case PoiProperty.Longitude:
                    poi.Location.Longitude = cell.GetValue<double>();
                    break;
                case PoiProperty.URL_Map:
                    break;
                default:
                    break;
            }
        }
        private static string GetPoiPropertyName(PoiProperty property) => property switch
        {
            PoiProperty.City => "Город",
            PoiProperty.Street => "Улица",
            PoiProperty.Latitude => "Latitude",
            PoiProperty.Longitude => "Longitude",
            _ => throw new Exception($"Не описано имя BoardProperty {property} для процедуры GetBoardPropertyName")
        };
        //public static List<PoiPropertySetter> GetDefaultColumns() => new List<PoiPropertySetter>(20)
        //    {
        //        new PoiPropertySetter( PoiProperty.Location),
        //        new PoiPropertySetter( PoiProperty.FormattedAddress),
        //        new PoiPropertySetter( PoiProperty.Description),
        //        //new PoiPropertySetter( PoiProperty.Latitude),
        //        //new PoiPropertySetter( PoiProperty.Longitude),
        //        //new PoiPropertySetter( PoiProperty.Country),
        //        //new PoiPropertySetter( PoiProperty.Region),
        //        //new PoiPropertySetter( PoiProperty.City),
        //        //new PoiPropertySetter( PoiProperty.District),
        //        //new PoiPropertySetter( PoiProperty.Street),
        //        //new PoiPropertySetter( PoiProperty.StreetNumber),
        //        //new PoiPropertySetter( PoiProperty.Zip),
                
        //        //new PoiPropertySetter( PoiProperty.URL_Map),
        //        //new PoiPropertySetter( PoiProperty.ProviderPlaceId)
        //    };
        public static List<PoiPropertySetter> GetDefaultColumnsSimple() => new List<PoiPropertySetter>(3)
        {
            new PoiPropertySetter( PoiProperty.Location),
            new PoiPropertySetter( PoiProperty.FormattedAddress),
            new PoiPropertySetter( PoiProperty.Description)
        };

    }
}
