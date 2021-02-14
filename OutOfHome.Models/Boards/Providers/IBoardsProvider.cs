using System;
using System.Threading.Tasks;

namespace OutOfHome.Models.Boards.Providers
{
    public interface IBoardsProvider<TBoardsContainer> where TBoardsContainer : IBoardsContainer
    {
        string FilesDirectory { get; set; }
        bool IsSavedFileExist { get; }
        string JsonFileName { get; }

        Task<TBoardsContainer> Load();
        Task<TBoardsContainer> Load(string fileName);
        void Save();
        void Save(string fileName);
        Task<DateTime> GetVersion(string fileName);
        

    }
}
