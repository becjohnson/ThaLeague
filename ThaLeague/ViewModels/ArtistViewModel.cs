using System.ComponentModel.DataAnnotations;
using ThaLeague.Models;

namespace ThaLeague.ViewModels
{
    public class ArtistViewModel
    {
        [Key]
        public int ArtistId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName
        {
            get => FullName = FirstName + " " + LastName;
            set { }
        }
        public string? StageName { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }
        public IFormFile? Image { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Spotify { get; set; }
        public string? Soundcloud { get; set; }
        public string? Youtube { get; set; }
        public string? Twitter { get; set; }
        public int? ColorPicker { get; set; }
        public bool DisplayName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IList<Video> Videos { get; set; } = new List<Video>();
        public IList<Audio> Audios { get; set; } = new List<Audio>();
    }
}
