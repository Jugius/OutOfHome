
namespace OutOfHome.Models.Boards.SupplierInfo
{
    public class OccupationStatus
    {       
        public virtual OccupationKind Kind { get; set; }       
        public virtual string Value 
        {
            get => _value ?? this.Kind.GetName();
            set { _value = string.IsNullOrEmpty(value) ? null : value; }
        }
        private string _value;
        public OccupationStatus(OccupationKind kind)
        {
            this.Kind = kind;
        }
    }
}
