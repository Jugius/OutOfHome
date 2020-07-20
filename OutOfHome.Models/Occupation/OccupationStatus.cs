using System.Collections.Generic;
using System.Drawing;

namespace OutOfHome.Models.Occupation
{
    public class OccupationStatus
    {       
        public virtual OccupationKind Kind { get; set; }       
        public virtual string Value {
            get => _value ?? this.Kind.GetName();
            set { _value = string.IsNullOrEmpty(value) ? null : value; }
        }
        private string _value;  
    }
}
