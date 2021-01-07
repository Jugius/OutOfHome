
using OutOfHome.Models.Boards.SupplierInfo;

namespace OutOfHome.Models.Boards
{
    public interface IHaveSupplierContent
    {
        OccupationInfo Occupation { get; set; }
        PriceInfo Price { get; set; }
    }
}
