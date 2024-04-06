using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicApp.Models.ViewModels
{
    public class ArtistViewModel
    {
        public Artist Artist { get; set; }
        public bool UpdateOrCreate { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AlbumList { get; set; }
    }
}
