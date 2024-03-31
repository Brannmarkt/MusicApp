using MusicApp.Areas.Identity.Data;
using MusicApp.Data;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Repositories
{
    public class UserRepository : GenericRepository<MusicAppUser>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        {
              
        }
    }
}
