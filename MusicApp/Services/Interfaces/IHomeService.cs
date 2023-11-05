using MusicApp.Models;

namespace MusicApp.Services.Interfaces
{
    public interface IHomeService
    {
        List<Artist> GetAllArtists();
        Artist GetArtist(Guid? id);
    }
}
