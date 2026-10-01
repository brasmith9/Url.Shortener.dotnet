using Microsoft.EntityFrameworkCore;
using Url.Shortner.Dtos;
using Url.Shortner.Entity;
using Url.Shortner.Models;

namespace Url.Shortner.Services;

public class UrlShortenerService(ILogger<UrlShortenerService> logger, AppDbContext dbContext) : IUrlShortenerService
{
    public async Task<ActivityResults<string?>> CreateShortUrlAsync(string host, string scheme, string path, ShortenUrlRequestDto request)
    {
        var code = request.CustomPath?.Trim();
        try
        {

            if (!string.IsNullOrWhiteSpace(code))
            {
                // Check if the custom path already exists in the database
                var customPathExist = await dbContext.ShortenedUrls.AnyAsync(u => u.Code == code);
                if (customPathExist)
                {
                    return new ActivityResults<string?>
                    {
                        Success = false,
                        Message = "Custom path already exists"
                    };
                }
            }
            
            code ??= GenerateUniqueCode(request.Url);
            
            var shortUrl = $"{scheme}://{host}/{code}";
            var res = await AddShortenedUrlRecord(shortUrl, request.Url, code);

            if (res <= 0) return new ActivityResults<string?>
            {
                Success = false,
                Message = $"Unable to create shortened url for {request.Url}"
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

    private string GenerateUniqueCode(string longUrl)
    {
        var code = string.Empty;
        return code;
    }

    private async Task<int> AddShortenedUrlRecord(string shortUrl, string longUrl, string code)
    {
        var shortenedUrl = new ShortenedUrl
        {
            ShortUrl = shortUrl,
            Url = longUrl,
            Code = code
        };
        await dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        var res = await dbContext.SaveChangesAsync();
        return res;
    }

    public async Task<ActivityResults<string?>> GetLongUrlAsync(string uniqueCode)
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
            Result = url.Url
        };
    }
}