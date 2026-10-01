namespace Url.Shortner.Models;

public class ActivityResults<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Result { get; set; } = default;
}