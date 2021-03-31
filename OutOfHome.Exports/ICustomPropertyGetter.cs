using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Exports
{
    public interface ICustomPropertyGetter : IPropertyGetter
    {
        string CustomProperty { get; set; }
    }
}
