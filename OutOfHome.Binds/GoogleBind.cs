using System;
using System.Collections.Generic;
using System.Text;

namespace OutOfHome.Binds
{
    public class GoogleBind : OutOfHome.Models.Binds.Bind
    {
        public IEnumerable<GoogleBindAddress> Addresses { get; set; }
    }
}
