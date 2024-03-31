using Microsoft.AspNetCore.Identity;
using MusicApp.Areas.Identity.Data;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MusicApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly SignInManager<MusicAppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(IUnitOfWork unitOfWork, SignInManager<MusicAppUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<MusicAppUser> GetAllUsers()
        {
            List<MusicAppUser>? usersList = _unitOfWork.User.GetAll().ToList();

            return usersList;
        }

        public MusicAppUser GetUser(string id)
        {
            MusicAppUser user = _unitOfWork.User.Get(u => u.Id == id);

            return user;
        }

        public async Task EditUserAsync(EditUserViewModel data)
        {
            var user = _unitOfWork.User.Get(u => u.Id == data.User.Id);

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            user.Email = data.User.Email;

            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
        }
    }
}
