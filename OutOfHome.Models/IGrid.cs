using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models
{
    public interface IGrid
    {        
        Task<List<Board>> GetBoards();
        Task<DateTime> GetDownloadedTime();
    }
}
