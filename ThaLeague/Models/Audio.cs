using System.ComponentModel.DataAnnotations;

namespace ThaLeague.Models
{
    public class Audio
    {
        [Key]
        public int AudioId { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public int? ArtistId { get; set; }
        public string? StageName { get; set; }
        public Artist? Artist { get; set; }
        public string? Featuring { get; set; }
        public string? Producer { get; set; }
        public string? Credits { get; set; }
        public string? Lyrics { get; set; }
        public string? Spotify { get; set; }
    }
}
