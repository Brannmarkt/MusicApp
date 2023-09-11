using MusicApp.Data;
using MusicApp.Models;
using MusicApp.Repositories.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace MusicApp.Repositories
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        private AppDbContext _context;
        public AlbumRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        
    }
}
