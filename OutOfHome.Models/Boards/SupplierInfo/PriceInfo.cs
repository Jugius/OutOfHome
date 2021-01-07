using System.Collections.Generic;
using System.Linq;

namespace OutOfHome.Models.Boards.SupplierInfo
{
    public class PriceInfo
    {
        public List<KeyValuePair<DateTimePeriod, float>> Prices { get; set; }
        public float Value { get; set; }
        public bool IsConstant { get; set; }
        public float GetValue(DateTimePeriod period)
        {
            if(this.IsConstant || this.Prices == null || this.Prices.Count == 0) return this.Value;
            return this.Prices.FirstOrDefault(a => !(period.End < a.Key.Start || period.Start > a.Key.End)).Value;
        }
    }
}
