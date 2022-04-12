using System.ComponentModel.DataAnnotations;
using ThaLeague.Models;

namespace ThaLeague.ViewModels
{
    public class AudioViewModel
    {
        [Key]
        public int AudioId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Title { get; set; }
        public IFormFile? Path { get; set; }
        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }
        public string? Featuring { get; set; }
        public string? Producer { get; set; }
        public string? Credits { get; set; }
        public string? Lyrics { get; set; }
        public string? Spotify { get; set; }
    }
}
