using Microsoft.AspNetCore.Identity;

namespace MusicApp.Services.Interfaces
{
    public interface IRoleService
    {
        List<IdentityRole> GetAllRoles();

    }
}
