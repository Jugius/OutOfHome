using OutOfHome.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OutOfHome.Exports.Excel.Models
{
    public sealed class Field
    {
        public FieldKind Kind { get; set; }
        public string Name => DefaultNames[this.Kind];
        public string NumberFormat
        {
            get
            {
                return this.Kind switch
                {
                    FieldKind.Price => "### ### ##0",
                    FieldKind.DoorsId => "########0",
                    FieldKind.OTS => "##0",
                    FieldKind.GRP => "##0.00",
                    _ => null,
                };
            }
        }
        public int Width
        {
            set { _width = value; }
            get { return _width != 0 ? _width : (_width = GetDefaultColumnWidth(this.Kind)); }
        }
        private int _width = 0;
        public string ColumnLetter
        {
            get => _columnLetter ?? string.Empty;
            set => _columnLetter = string.IsNullOrEmpty(value) ? null : value;
        }
        private string _columnLetter;
        public bool IsHyperlink { get; set; } = false;
        public Field(FieldKind kind) { this.Kind = kind; }
        public Field() { }
        public object GetPropertyValueFrom(Board board)
        {
            return this.Kind switch
            {
                FieldKind.ProviderID => board.ProviderID,
                FieldKind.Provider => board.Provider,
                FieldKind.Supplier => board.Supplier,
                FieldKind.SupplierCode => board.SupplierCode,
                FieldKind.City => board.Address.City,
                FieldKind.Address => board.Address.FormattedAddress,
                FieldKind.Side => board.Side,
                FieldKind.Kind => board.Type,
                FieldKind.Size => board.Size,
                FieldKind.URL_Photo => board.Photo,
                FieldKind.URL_Map => board.Map,
                FieldKind.Light => board.Lighting ? "+" : "-",
                FieldKind.Price => (board as IHaveSupplierContent)?.Price,
                FieldKind.DoorsId => board.DoorsInfo?.DoorsID,
                FieldKind.OTS => board.DoorsInfo?.OTS,
                FieldKind.GRP => board.DoorsInfo?.GRP,
                FieldKind.Location => board.Location?.ToString(),
                FieldKind.Street => board.Address.Street,
                FieldKind.StreetNumber => board.Address.StreetNumber,
                FieldKind.AddressDescription => board.Address.Description,
                FieldKind.OccSource => throw new NotImplementedException(),
                _ => throw new Exception($"There is no implemented getter in GetPropertyValueFrom for FieldKind {this.Kind}"),
            };
        }

        static readonly Dictionary<FieldKind, string> DefaultNames = new Dictionary<FieldKind, string>()
        {
            { FieldKind.ProviderID, "код" },
            { FieldKind.Provider, "источник" },
            { FieldKind.Supplier, "оператор" },
            { FieldKind.SupplierCode, "код оператора" },
            { FieldKind.City, "город" },
            { FieldKind.Address, "адрес" },
            { FieldKind.Kind, "тип" },
            { FieldKind.Side, "сторона" },
            { FieldKind.Size, "размер" },
            { FieldKind.Light, "свет" },
            { FieldKind.Price, "прайс" },
            { FieldKind.URL_Map, "схема" },
            { FieldKind.URL_Photo, "фото" },
            { FieldKind.DoorsId, "DoorsID" },
            { FieldKind.OTS, "OTS" },
            { FieldKind.GRP, "GRP" },
            { FieldKind.Location, "коорд" },
            { FieldKind.OccSource, "occup" }
        };
        private static int GetDefaultColumnWidth(FieldKind kind)
        {
            switch (kind)
            {
                case FieldKind.ProviderID:
                case FieldKind.SupplierCode:
                case FieldKind.City:
                case FieldKind.OccSource:
                    return 15;

                case FieldKind.Provider:
                case FieldKind.Supplier:
                    return 17;

                case FieldKind.Address:
                    return 60;

                case FieldKind.Side:
                case FieldKind.Size:
                case FieldKind.Light:
                case FieldKind.OTS:
                case FieldKind.GRP:
                    return 7;

                default: return 8;
            }
        }
    }
}
