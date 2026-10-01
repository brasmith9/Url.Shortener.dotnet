using Url.Shortner.Dtos;
using Url.Shortner.Models;

namespace Url.Shortner.Services;

public interface IUrlShortenerService
{
    Task<ActivityResults<string?>> CreateShortUrlAsync(string host, string scheme, string path, ShortenUrlRequestDto request);
    
    Task<ActivityResults<string?>> GetLongUrlAsync(string uniqueCode);
}