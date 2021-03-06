﻿using OutOfHome.Models.Boards;
using System.Collections.Generic;

namespace OutOfHome.Exports.Excel.DocumentModel.Fields
{
    public sealed class BoardExcelField : BoardPropertyGetter, IExcelField
    {
        private int _columnWidth = 0;
        private string _columnHeader;
        private static readonly HashSet<BoardProperty> PropertiesContainsHyperlinks = new HashSet<BoardProperty>
        {
            BoardProperty.URL_DoorsMap, BoardProperty.URL_DoorsPhoto, BoardProperty.URL_GoogleMapPoint, BoardProperty.URL_Map, BoardProperty.URL_Photo, BoardProperty.URL_StreetsView
        };

        public string ColumnHeader
        {
            get => _columnHeader ?? DefaultNames[this.Kind];
            set => _columnHeader = string.IsNullOrEmpty(value) ? null : value;
        }
        public int ColumnWidth
        {
            set { _columnWidth = value; }
            get { return _columnWidth != 0 ? _columnWidth : (_columnWidth = GetDefaultColumnWidth(this.Kind)); }
        }
        
        public bool IsHyperlink { get; set; }
        public BoardExcelField(BoardProperty property) : base(property)
        {
            this.IsHyperlink = PropertiesContainsHyperlinks.Contains(property);
        }
        public string NumberFormat => this.Kind switch
        {
            BoardProperty.Price => "### ### ##0",
            BoardProperty.DoorsId => "########0",
            BoardProperty.OTS => "##0",
            BoardProperty.GRP => "##0.00",
            _ => null,
        };

        public static List<BoardExcelField> GetDefaultColumns() => new List<BoardExcelField>(20)
            {
                new BoardExcelField(BoardProperty.Supplier),
                new BoardExcelField(BoardProperty.SupplierCode),
                new BoardExcelField(BoardProperty.Region),
                new BoardExcelField(BoardProperty.City),
                new BoardExcelField(BoardProperty.Address),
                new BoardExcelField(BoardProperty.Kind),
                new BoardExcelField(BoardProperty.Size),
                new BoardExcelField(BoardProperty.Side),
                new BoardExcelField(BoardProperty.Light),
                new BoardExcelField(BoardProperty.URL_Photo),
                new BoardExcelField(BoardProperty.URL_Map),
                new BoardExcelField(BoardProperty.URL_DoorsPhoto),                
                new BoardExcelField(BoardProperty.URL_GoogleMapPoint),
                new BoardExcelField(BoardProperty.Price),
                new BoardExcelField(BoardProperty.DoorsId),
                new BoardExcelField(BoardProperty.OTS),
                new BoardExcelField(BoardProperty.GRP),
                new BoardExcelField(BoardProperty.Provider),
            };

        private static int GetDefaultColumnWidth(BoardProperty kind)
        {
            return kind switch
            {
                BoardProperty.ProviderID or BoardProperty.SupplierCode or BoardProperty.City or BoardProperty.OccSource => 15,
                BoardProperty.Provider or BoardProperty.Supplier => 17,
                BoardProperty.Address => 60,
                BoardProperty.Side or BoardProperty.Size or BoardProperty.Light or BoardProperty.OTS or BoardProperty.GRP => 7,
                _ => 8,
            };
        }
        private static readonly Dictionary<BoardProperty, string> DefaultNames = new Dictionary<BoardProperty, string>()
        {
            { BoardProperty.ProviderID, "ID" },
            { BoardProperty.Provider, "источник" },
            { BoardProperty.Supplier, "оператор" },
            { BoardProperty.SupplierCode, "код оператора" },
            { BoardProperty.Region, "область" },
            { BoardProperty.City, "город" },
            { BoardProperty.Address, "адрес" },
            { BoardProperty.Kind, "тип" },
            { BoardProperty.Side, "сторона" },
            { BoardProperty.Size, "размер" },
            { BoardProperty.Light, "свет" },
            { BoardProperty.Price, "прайс" },
            { BoardProperty.URL_Map, "схема" },
            { BoardProperty.URL_Photo, "фото" },
            { BoardProperty.URL_DoorsPhoto, "фото Doors"},
            { BoardProperty.URL_DoorsMap, "схема Doors" },
            { BoardProperty.URL_GoogleMapPoint, "карта" },
            { BoardProperty.URL_StreetsView, "панорама"},
            { BoardProperty.DoorsId, "DoorsID" },
            { BoardProperty.OTS, "OTS" },
            { BoardProperty.GRP, "GRP" },
            { BoardProperty.Location, "коорд" },
            { BoardProperty.Angle, "угол" },
            { BoardProperty.OccSource, "занятость ориг. стр." },
            { BoardProperty.Street, "улица" },
            { BoardProperty.StreetNumber, "дом" },
            { BoardProperty.AddressDescription, "описание"}
        };
    }  
}
