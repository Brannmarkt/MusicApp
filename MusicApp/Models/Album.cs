using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApp.Models
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string AlbumTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        [ValidateNever]
        public string? Description { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        [ValidateNever]
        public List<Song>? Songs { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        [ValidateNever]
        public Artist Artist { get; set; }

    }
}
