namespace ThaLeague.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
        public string? StageName { get; set; }
    }
}
