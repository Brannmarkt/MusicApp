using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApp.Models
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }
        public string AlbumTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public List<Song> Songs { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }

    }
}
