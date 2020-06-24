using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models
{
    public abstract class OccupationInfo
    {
        public abstract OccupationStatus GetStatus(int month);
    }
}
