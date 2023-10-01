using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApp.Models
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string SongTitle { get; set; }
        [ValidateNever]
        public string? Lyrics { get; set; }
        [ValidateNever]
        public byte[]? Content { get; set; }
        [ValidateNever]
        public string? ContentType { get; set; }
        public Guid AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        [ValidateNever]
        public Album Album { get; set; }

    }
}
