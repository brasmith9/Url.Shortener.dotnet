namespace Url.Shortner.Dtos;

public sealed class ShortenUrlResponseDto
{
    public string Url { get; init; } = string.Empty;
    public string ShortUrl { get; init; } = string.Empty;
}