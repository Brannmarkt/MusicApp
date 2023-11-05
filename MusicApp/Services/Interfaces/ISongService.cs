using MusicApp.Models;
using MusicApp.Models.ViewModels;

namespace MusicApp.Services.Interfaces
{
    public interface ISongService
    {
        Song GetSong(Guid? id);
        void CreateSong(SongViewModel songViewModel, IFormFile? file);
        void EditSong(SongViewModel songViewModel, IFormFile? file);
        void DeleteSong(Guid? id);
    }
}
