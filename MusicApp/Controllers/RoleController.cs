using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

using Constants = MusicApp.Data.Constants;

namespace MusicApp.Controllers
{
    public class RoleController : Controller
    {
        [Authorize(Roles = $"{Constants.Roles.User}")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy = "RequireManager")]
        [Authorize(Roles = $"{Constants.Roles.Manager}")]
        public IActionResult Manager()
        {
            return View();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
