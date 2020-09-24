using System;
using System.Collections.Generic;
using System.Text;

namespace OutOfHome.Helpers
{
    internal static partial class Dictionaries
    {
        
        internal static Dictionary<string, string> DictionaryTypes = new Dictionary<string, string>
        {
            { "Digital", "Цифровая панель" },
            { "Щит вертикаль", "Щит" },
            { "Унипол", "Щит" },
            { "Призма вертикаль", "Призма" },
            { "Призмавижн", "Призма" },
            { "Беклайт", "Бэклайт" }
        };
    }
}
