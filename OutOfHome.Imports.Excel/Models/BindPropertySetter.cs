using OfficeOpenXml;
using OutOfHome.Models;
using OutOfHome.Models.Binds;
using System;
using System.Collections.Generic;

namespace OutOfHome.Imports
{
    public class BindPropertySetter : PropertySetter
    {
        public BindProperty Kind { get; set; }
        private bool Required
        {
            get {
                switch(this.Kind)
                {
                    case BindProperty.City:
                    case BindProperty.Street:
                    case BindProperty.Latitude:
                    case BindProperty.Longitude:
                        return true;
                    default: return false;

                }
            }
        }
        public BindPropertySetter(BindProperty kind)
        {
            this.Kind = kind;
        }
        public void SetValue(Bind bind, ExcelRange cell)
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
                throw new Exception($"Недопустимо пустое значение ячейки для столбца {GetBindPropertyName(this.Kind)}. Адрес ячейки {cell.Address}");

            switch(this.Kind)
            {
                case BindProperty.Provider:
                    break;
                case BindProperty.ProviderPlaceId:
                    bind.Address.PlaceId = val;
                    break;
                case BindProperty.OriginalAddress:
                    bind.OriginalAddress = val;
                    break;
                case BindProperty.Description:
                    bind.Description = val;
                    break;
                case BindProperty.Country:
                    bind.Address.Country = val;
                    break;
                case BindProperty.Region:
                    bind.Address.Region = val;
                    break;
                case BindProperty.City:
                    bind.Address.City = val;
                    break;
                case BindProperty.District:
                    bind.Address.District = val;
                    break;
                case BindProperty.Zip:
                    bind.Address.Zip = val;
                    break;
                case BindProperty.Street:
                    bind.Address.Street = val;
                    break;
                case BindProperty.StreetNumber:
                    bind.Address.StreetNumber = val;
                    break;
                case BindProperty.FormattedAddress:
                    //bind.Address.FormattedAddress = val;
                    break;
                case BindProperty.Intersection:
                    bind.Address.Intersection = val;
                    break;
                case BindProperty.Location:
                    if(OutOfHome.Models.Location.TryParse(val, out Location l))
                        bind.Address.Location = l;
                    else throw new Exception($"Ошибка чтения координат в ячейке {cell.Address}. Значение: {val}");
                    break;
                case BindProperty.Latitude:
                    bind.Address.Location.Latitude = cell.GetValue<double>();
                    break;
                case BindProperty.Longitude:
                    bind.Address.Location.Longitude = cell.GetValue<double>();
                    break;
                case BindProperty.URL_Map:
                    break;
                default:
                    break;
            }
        }
        private static string GetBindPropertyName(BindProperty property) => property switch
        {
            BindProperty.City => "Город",
            BindProperty.Street => "Улица",
            BindProperty.Latitude => "Latitude",
            BindProperty.Longitude => "Longitude",
            _ => throw new Exception($"Не описано имя BoardProperty {property} для процедуры GetBoardPropertyName")
        };
        public static List<BindPropertySetter> GetDefaultColumns() => new List<BindPropertySetter>(20)
            {
                new BindPropertySetter( BindProperty.OriginalAddress),
                new BindPropertySetter( BindProperty.Latitude),
                new BindPropertySetter( BindProperty.Longitude),
                new BindPropertySetter( BindProperty.Country),
                new BindPropertySetter( BindProperty.Region),
                new BindPropertySetter( BindProperty.City),
                new BindPropertySetter( BindProperty.District),
                new BindPropertySetter( BindProperty.Street),
                new BindPropertySetter( BindProperty.StreetNumber),
                new BindPropertySetter( BindProperty.Zip),
                new BindPropertySetter( BindProperty.Description),
                new BindPropertySetter( BindProperty.URL_Map),
                new BindPropertySetter( BindProperty.ProviderPlaceId)
            };

    }
}
