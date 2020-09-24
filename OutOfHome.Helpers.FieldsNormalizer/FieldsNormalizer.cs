using OutOfHome.Models;
using System.Collections.Generic;

namespace OutOfHome.Helpers
{
    public static class FieldsNormalizer
    {
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

    }
}
