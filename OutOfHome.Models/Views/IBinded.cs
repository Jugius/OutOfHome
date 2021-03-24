using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models.Views
{
    public interface IBinded
    {
        Pois.Poi Poi { get; set; }
    }
}
