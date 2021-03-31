using OfficeOpenXml;
using OutOfHome.Imports.Excel;
using OutOfHome.Models;
using OutOfHome.Models.Boards;
using OutOfHome.Models.Views;
using System;
using System.Collections.Generic;

namespace OutOfHome.Imports
{
    public class BoardPropertySetter : PropertySetter
    {
        private static readonly HashSet<BoardProperty> _required = new() { BoardProperty.Supplier, BoardProperty.City, BoardProperty.Street, BoardProperty.Side, BoardProperty.Kind, BoardProperty.Size, BoardProperty.Location };
        public BoardProperty Kind { get; }
        public int ColumnIndex { get; set; }
        private bool Required => _required.Contains(this.Kind);
        public BoardPropertySetter(BoardProperty kind)
        {
            this.Kind = kind;
        }
        public void SetValue(ExcelBoard board, ExcelRange cell)
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
                throw new Exception($"Недопустимо пустое значение ячейки для столбца {GetBoardPropertyName(this.Kind)}. Адрес ячейки {cell.Address}");

            switch(this.Kind)
            {
                case BoardProperty.ProviderID:
                    board.ProviderID = val;
                    break;
                case BoardProperty.Provider:
                    board.Provider = val;
                    break;
                case BoardProperty.Supplier:
                    board.Supplier = val;
                    break;
                case BoardProperty.SupplierCode:
                    board.SupplierCode = val;
                    break;
                case BoardProperty.Region:
                  if(string.IsNullOrEmpty(board.Address.City.Region) && !string.IsNullOrEmpty(val))
                        board.Address.City.Region = val;
                    break;

                case BoardProperty.City:
                    
                    var city = Helpers.Cities.Defaults.GetCityByName(val);
                    
                    if(city == null)
                        board.Address.City.Name = val;
                    else
                        board.Address.City = city;
                    break;

                case BoardProperty.Street:
                    board.Address.Street = val;
                    break;
                case BoardProperty.StreetNumber:
                    board.Address.StreetNumber = val;
                    break;
                case BoardProperty.AddressDescription:
                    board.Address.Description = val;
                    break;
                case BoardProperty.Side:
                    board.Side = val;
                    break;
                case BoardProperty.Kind:
                    board.Type = val;
                    break;
                case BoardProperty.Size:
                    board.Size = val;
                    break;
                //case BoardProperty.Light:
                //    board.Lighting = 
                //    break;
                case BoardProperty.Location:
                    board.Location = Location.Parse(val);             
                    break;
                case BoardProperty.URL_Photo:
                    board.Photo = cell.GetUri();
                    break;
                case BoardProperty.URL_Map:
                    board.Map = cell.GetUri();
                    break;
                case BoardProperty.URL_DoorsPhoto:
                    (board.DoorsInfo ??= new DoorsInfo()).Photo = cell.GetUri();                    
                    break;
                case BoardProperty.URL_DoorsMap:
                    (board.DoorsInfo ??= new DoorsInfo()).Map = cell.GetUri();
                    break;                
                case BoardProperty.DoorsId:
                    (board.DoorsInfo ??= new DoorsInfo()).DoorsID = cell.GetValue<int>();
                    break;
                case BoardProperty.OTS:
                    (board.DoorsInfo ??= new DoorsInfo()).OTS = cell.GetValue<int>();
                    break;
                case BoardProperty.GRP:
                    (board.DoorsInfo ??= new DoorsInfo()).GRP = cell.GetValue<float>();
                    break;
                case BoardProperty.Price:
                    if(board is IHaveSupplierContent b)
                        b.Price = new Models.Boards.SupplierInfo.PriceInfo { Value = cell.GetValue<int>(), IsConstant = true };
                    break;
                //case BoardProperty.Color:
                //    if(board is IColored c)
                //        c.Color = cell.GetBackgroundColor();
                    //break;
            }
        }
        private static string GetBoardPropertyName(BoardProperty property) => property switch 
        {
            BoardProperty.Supplier => "Оператор",
            BoardProperty.City => "Город",
            BoardProperty.Street => "Улица",
            BoardProperty.Side => "Сторона",
            BoardProperty.Kind => "Формат",
            BoardProperty.Size => "Размер",
            BoardProperty.Location => "Координаты",
            _ => throw new Exception($"Не описано имя BoardProperty {property} для процедуры GetBoardPropertyName")
        };
        public static List<BoardPropertySetter> GetDefaultColumns()
        {
            var list = new List<BoardPropertySetter>(20)
            {
                 new BoardPropertySetter(BoardProperty.Region),
                 new BoardPropertySetter(BoardProperty.City),
                 new BoardPropertySetter(BoardProperty.Supplier),
                 new BoardPropertySetter(BoardProperty.SupplierCode),
                 new BoardPropertySetter(BoardProperty.Street),
                 new BoardPropertySetter(BoardProperty.StreetNumber),
                 new BoardPropertySetter(BoardProperty.AddressDescription),
                 new BoardPropertySetter(BoardProperty.Kind),
                 new BoardPropertySetter(BoardProperty.Size),
                 new BoardPropertySetter(BoardProperty.Side),
                 new BoardPropertySetter(BoardProperty.DoorsId),
                 new BoardPropertySetter(BoardProperty.OTS),
                 new BoardPropertySetter(BoardProperty.GRP),
                 new BoardPropertySetter(BoardProperty.URL_Photo),
                 new BoardPropertySetter(BoardProperty.URL_Map),
                 new BoardPropertySetter(BoardProperty.Price),
                 new BoardPropertySetter(BoardProperty.Location),
                 //new BoardPropertySetter(OutOfHome.Models.Boards.BoardProperty.Color)
            };
            int index = 0;
            foreach(var item in list)
            {
                item.ColumnIndex = ++index;
            }
            return list;
        }
    }
}
