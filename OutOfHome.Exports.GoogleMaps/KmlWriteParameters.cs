using System;

namespace OutOfHome.Exports.GoogleMaps
{
    public sealed class KmlWriteParameters
    {
        public enum PreferredSource { Doors, Supplier }
        public GroupingProperty GroupingProperty
        {
            get => _groupingProperty;
            set
            {
                _groupingProperty = value;
                _layersSelector = value.GetGroupSelector();
            }
        }
        private GroupingProperty _groupingProperty = GroupingProperty.None;
        public Func<ColoredBoard, string> LayersSelector
        {
            get
            {
                if (this.GroupingProperty == GroupingProperty.None)
                    return null;

                return this._layersSelector ??= this.GroupingProperty.GetGroupSelector();
            }
        }
        private Func<ColoredBoard, string> _layersSelector;

        public bool CreateLayers { get; set; }

        public bool NameCity { get; set; } = true;
        public bool NameCode { get; set; } = true;
        public bool NameAddress { get; set; } = true;
        public bool CardType { get; set; } = true;
        public bool CardSide { get; set; } = true;
        public bool CardMedia { get; set; } = true;
        public bool CardSupplier { get; set; }
        public bool CardCode { get; set; }

        public PreferredSource PreferredPhoto { get; set; } = PreferredSource.Doors;
        public string FilePath { get; set; }
    }
}
