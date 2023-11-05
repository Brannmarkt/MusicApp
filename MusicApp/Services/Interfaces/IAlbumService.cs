using MusicApp.Models;
using MusicApp.Models.ViewModels;

namespace MusicApp.Services.Interfaces
{
    public interface IAlbumService
    {
        List<Album> GetAllAlbums();
        Album GetAlbum(Guid? id);
        void CreateAlbum(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive);
        void EditAlbum(AlbumViewModel albumViewModel, IFormFile? file, IFormFile? archive);
        void DeleteAlbum(Guid? id);
        byte[] DownloadArchive(Album album);
    }
}
