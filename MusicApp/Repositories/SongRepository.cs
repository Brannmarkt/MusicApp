using MusicApp.Data;
using MusicApp.Models;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Repositories
{
    public class SongRepository : GenericRepository<Song>, ISongRepository
    {
        public SongRepository(AppDbContext context) : base(context)
        {
                
        }

    }
}
