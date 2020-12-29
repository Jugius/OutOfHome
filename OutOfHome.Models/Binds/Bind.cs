using System.Collections.Generic;

namespace OutOfHome.Models.Binds
{
    public class Bind
    {
        public string OriginalAddress { get; set; }
        public string Description { get; set; }
        public BindAddress Address { get; set; }
    }
}
