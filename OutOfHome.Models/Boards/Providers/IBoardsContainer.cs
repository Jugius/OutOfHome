using System;
using System.Collections.Generic;

namespace OutOfHome.Models.Boards.Providers
{
    public interface IBoardsContainer
    {
        IEnumerable<Board> Boards { get; set; }
        DateTime Loaded { get; set; }
    }
}
