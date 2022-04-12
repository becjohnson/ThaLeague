using System.ComponentModel.DataAnnotations;

namespace ThaLeague.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string FullName
        {
            get => FullName = FirstName + " " + LastName;
            set { }
        }
        [Display(Name = "Stage Name")]
        public string? StageName { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Spotify { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? Twitter { get; set; }
        public string? Soundcloud { get; set; }
        public string? Youtube { get; set; }
        public int? ColorPicker { get; set; }
        [Display(Name = "Display Name?")]
        public bool DisplayName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public IList<Video> Videos { get; set; } = new List<Video>();

        public IList<Audio> Audios { get; set; } = new List<Audio>();
    }
}
