using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ArtistName { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        public string Country { get; set; }
        [ValidateNever]
        public string? Description { get; set; }
        [ValidateNever]
        public List<Album>? Albums { get; set; }

    }
}
