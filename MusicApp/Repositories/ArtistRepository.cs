using MusicApp.Data;
using MusicApp.Models;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(AppDbContext context) : base(context)
        {
           
        }


    }
}
