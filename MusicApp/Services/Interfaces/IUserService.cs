using MusicApp.Areas.Identity.Data;
using MusicApp.Models.ViewModels;

namespace MusicApp.Services.Interfaces
{
    public interface IUserService
    {
        List<MusicAppUser> GetAllUsers();
        MusicAppUser GetUser(string id);
        Task EditUserAsync(EditUserViewModel user);
    }
}
