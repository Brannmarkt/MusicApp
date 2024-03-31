using Microsoft.AspNetCore.Identity;
using MusicApp.Data;
using MusicApp.Repositories.Interfaces;

namespace MusicApp.Repositories
{
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        
        public RoleRepository(AppDbContext context) : base(context) 
        {
            
        }
    }
}
