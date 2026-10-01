using Microsoft.EntityFrameworkCore;
using Url.Shortner.Entity;
using Url.Shortner.Models;

namespace Url.Shortner.Services;

public class UrlShortenerService(ILogger<UrlShortenerService> logger, AppDbContext dbContext) : IUrlShortenerService
{
    public async Task<ActivityResults<string?>> CreateShortUrlAsync(string host, string scheme, string path,
        string longUrl)
    {
        try
        {
            var shortenedUrl = new ShortenedUrl();
            await dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            var res = await dbContext.SaveChangesAsync();

            if (res <= 0) return new ActivityResults<string?>
            {
                Success = false,
                Message = $"Url {shortenedUrl.ShortUrl} already exists"
            };
            return new ActivityResults<string?>
            {
                Success = true,
                Message = "Success",
                Result = $"{scheme}://{host}{path}"
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating shortened url");
            return new ActivityResults<string?>
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    public async Task<ActivityResults<string?>> GetShortUrlAsync(string uniqueCode)
    {
        var url = await dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.Code == uniqueCode);

        if (url is null)
            return new ActivityResults<string?>
            {
                Success = false,
                Message = "Url not found"
            };

        return new ActivityResults<string?>
        {
            Success = true,
            Message = "Success",
            Result = url.ShortUrl
        };
    }
}