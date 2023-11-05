using MusicApp.Models;
using MusicApp.Models.ViewModels;

namespace MusicApp.Services.Interfaces
{
    public interface IArtistService
    {
        List<Artist> GetAllArtists();
        Artist GetArtist(Guid? id);
        void UpsertArtist(ArtistViewModel artistViewModel, IFormFile? file);
        void DeleteArtist(Guid? id);
    }
}
