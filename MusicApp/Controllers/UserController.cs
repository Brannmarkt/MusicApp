using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using MusicApp.Areas.Identity.Data;
using MusicApp.Models.ViewModels;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Controllers
{
    public class UserController : Controller
    {
        public readonly IUserService _userService;
        public readonly IRoleService _roleService;
        public readonly SignInManager<MusicAppUser> _signInManager;

        public UserController(IUserService userService, IRoleService roleService, SignInManager<MusicAppUser> signInManager)
        {
            _userService = userService;
            _roleService = roleService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _userService.GetUser(id); 
            var roles = _roleService.GetAllRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

            var vm = new EditUserViewModel { User = user, Roles = roleItems };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel editUserViewModel)
        {
            if(ModelState.IsValid)
            {
                await _userService.EditUserAsync(editUserViewModel);

                return RedirectToAction("Edit", new { id = editUserViewModel.User.Id });
            }

            return RedirectToAction("Edit", new {id = editUserViewModel.User.Id});
        }
    }
}
