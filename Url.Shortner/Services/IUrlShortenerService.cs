using Url.Shortner.Models;

namespace Url.Shortner.Services;

public interface IUrlShortenerService
{
    Task<ActivityResults<string?>> CreateShortUrlAsync(string host, string scheme, string path, string longUrl);
    
    Task<ActivityResults<string?>> GetShortUrlAsync(string uniqueCode);
}