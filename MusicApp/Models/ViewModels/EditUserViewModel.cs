using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using MusicApp.Areas.Identity.Data;

namespace MusicApp.Models.ViewModels
{
    public class EditUserViewModel
    {
        public MusicAppUser User { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
