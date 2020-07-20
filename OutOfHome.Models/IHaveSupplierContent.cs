
using OutOfHome.Models.Occupation;

namespace OutOfHome.Models
{
    public interface IHaveSupplierContent
    {
        OccupationInfo Occupation { get; set; }
        int Price { get; set; }
    }
}
