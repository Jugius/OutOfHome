using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models.Views
{
    public abstract class BaseBindModelView
    {
        public virtual BaseBoardModelView Board { get; set; }
        public virtual BasePoiModelView Poi { get; set; }
    }
}
