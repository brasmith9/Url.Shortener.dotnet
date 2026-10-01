namespace Url.Shortner.Entity;

public class ShortenedUrl
{
    public Guid Id { get; set; }
    
    public string Url { get; set; }
    
    public string ShortUrl { get; set; }
    
    public string Code { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}