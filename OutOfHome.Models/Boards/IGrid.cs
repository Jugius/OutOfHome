using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models.Boards
{
    public interface IGrid
    {        
        Task<List<Board>> GetBoards();
        Task<DateTime> GetDownloadedTime();
    }
}
