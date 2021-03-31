using OutOfHome.Models.Boards;
using OutOfHome.Models.Views;

namespace OutOfHome.Exports
{
    public static class Extentions
    {
        
        public static string GetFormattedAddress(this BoardAddress address)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(address.Street);

            if(!string.IsNullOrEmpty(address.StreetNumber))
                _ = builder.Append(", ").Append(address.StreetNumber);

            if(!string.IsNullOrEmpty(address.Description))
                _ = builder.Append(", ").Append(char.IsUpper(address.Description[0]) ? (char.ToLower(address.Description[0]) + address.Description[1..]) : address.Description);

            return builder.ToString();
        }

        public static string GetFormattedAddress(this BaseBoardModelView board)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(board.Street);

            if(!string.IsNullOrEmpty(board.StreetHouse))
                _ = builder.Append(", ").Append(board.StreetHouse);

            if(!string.IsNullOrEmpty(board.AddressDescription))
                _ = builder.Append(", ").Append(
                    char.IsUpper(board.AddressDescription[0])
                    ? (char.ToLower(board.AddressDescription[0]) + board.AddressDescription[1..])
                    : board.AddressDescription);

            return builder.ToString();
        }
    }
}
