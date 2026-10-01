using Microsoft.AspNetCore.Mvc;
using Url.Shortner.Dtos;
using Url.Shortner.Models;
using Url.Shortner.Services;

namespace Url.Shortner.Extensions;

public static class ApiEndpointCollectionExtensions
{
    public static void AddApiEndpoints(this WebApplication app)
    {
        app.MapPost("shorten", ShortenUrlHandler)
            .WithName("ShortenUrl")
            .WithOpenApi();

        app.MapGet("{code}", RedirectToLongUrlHandler)
            .WithName("RedirectToLongUrl")
            .WithOpenApi();
    }
    
    private static async Task<IResult> ShortenUrlHandler([FromBody] ShortenUrlRequestDto request, HttpContext ctx, ILogger<Endpoint> logger,
        IUrlShortenerService urlShortenerService)
    {
        logger.LogInformation("Shorten Url Request");
        
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest(new ApiResponse<ShortenUrlResponseDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Invalid Url"
            });
        }

        var shortUrlResult = await urlShortenerService.CreateShortUrlAsync(ctx.Request.Host.Host,
            ctx.Request.Scheme, ctx.Request.Path, request);
        return Results.BadRequest(new ApiResponse<string>
        {
            Code = StatusCodes.Status200OK,
            Message = "Success",
            Data = shortUrlResult.Result
        });
    }

    
    private static async Task<IResult> RedirectToLongUrlHandler([FromRoute] string code, HttpContext ctx, ILogger<Endpoint> logger,
        IUrlShortenerService urlShortenerService)
    {
        logger.LogInformation("Redirecting to Long Url");

        var shortUrlResult = await urlShortenerService.GetLongUrlAsync(code);
        
        if(!shortUrlResult.Success)
            return Results.Redirect("/");

        return Results.Redirect(shortUrlResult.Result!);
    }
}