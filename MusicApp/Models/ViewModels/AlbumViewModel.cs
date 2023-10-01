using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicApp.Models.ViewModels
{
    public class AlbumViewModel
    {
        public Album Album { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SongList { get; set; }
    }
}
