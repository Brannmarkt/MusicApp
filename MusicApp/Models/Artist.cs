using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }
        public string ArtistName { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }

    }
}
