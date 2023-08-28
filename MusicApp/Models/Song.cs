using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApp.Models
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; }
        public string SongTitle { get; set; }
        public string Lyrics { get; set; }
        public byte[] Content { get; set; }

        public Guid AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }
    }
}
