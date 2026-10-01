namespace Url.Shortner.Dtos;

public sealed class ShortenUrlRequestDto
{
    public string Url { get; init; } =  string.Empty;
    
    public string? CustomPath { get; init; }
}