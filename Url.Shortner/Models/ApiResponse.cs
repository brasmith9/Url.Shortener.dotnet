namespace Url.Shortner.Models;

public class ApiResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public int Code { get; set; }
    public T? Data { get; set; } = default;
}