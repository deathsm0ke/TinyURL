namespace WebApplication1.Models
{
    public class TinyUrl
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty; // Requirement: 6 characters
        public bool IsPrivate { get; set; } // Requirement: Mark as Private
        public int ClickCount { get; set; } = 0; // Requirement: Show total clicks
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
