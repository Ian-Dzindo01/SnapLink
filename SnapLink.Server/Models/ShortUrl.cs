namespace SnapLink.Server.Models
{
    public class ShortUrl
    {
        public string ShortenedUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string? CustomAlias { get; set; }
    }
}
