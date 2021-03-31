using OutOfHome.Models.Boards;
using System;
using System.Collections.Generic;

namespace OutOfHome.DataProviders.Boards
{
    public interface IBoardsContainer
    {
        IEnumerable<Board> Boards { get; set; }
        DateTime Loaded { get; set; }
    }
}
