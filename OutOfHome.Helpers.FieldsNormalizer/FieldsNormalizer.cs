using OutOfHome.Models;
using OutOfHome.Models.Boards;
using System.Collections.Generic;

namespace OutOfHome.Helpers
{
    public static class FieldsNormalizer
    {
        private static readonly System.Text.RegularExpressions.Regex DeleteSpaces = new System.Text.RegularExpressions.Regex(@"\s+");
        public static string GetNormalizedType(Board board)
        {
            return Dictionaries.DictionaryTypes.TryGetValue(board.Type, out string val) ? val : null;
        }
        public static bool TryGetNormalizedType(Board board, out string type)
        {
            return Dictionaries.DictionaryTypes.TryGetValue(board.Type, out type);
        }
        
        public static string GetNormalizedSize(Board board)
        {
            return Dictionaries.DictionarySizes.TryGetValue(board.Size, out string val) ? val : null;
        }
        public static bool TryGetNormalizedSize(Board board, out string size) => Dictionaries.DictionarySizes.TryGetValue(board.Size, out size);

        public static void NormalizeSize(this Board board)
        {
            string newSize = board.Size.Trim().ToLower().Replace(',', '.').Replace('х', 'x');
            board.Size = Dictionaries.DictionarySizes.TryGetValue(newSize, out string val) ? val : newSize;
        }
        public static void NormalizeAddressString(this Board board)
        {
            if(board.Address.Street == null)
                return;
            string tmp = board.Address.Street.Trim();
            board.Address.Street = DeleteSpaces.Replace(tmp, " ");            
        }
        public static void NormalizeType(this Board board)
        {
            if(Dictionaries.DictionaryTypes.TryGetValue(board.Type, out string type))
                board.Type = type;
        }
        public static void NormalizeFields(this Board board)
        {
            board.NormalizeSize();
            board.NormalizeAddressString();
            board.NormalizeType();
        }
    }
}
