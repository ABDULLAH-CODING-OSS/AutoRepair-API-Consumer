namespace ERP_Consumer.Helpers;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse<T> Ok(T data) =>
        new() { Success = true, Data = data, StatusCode = 200 };

    public static ApiResponse<T> Fail(string message, int statusCode = 500) =>
        new() { Success = false, ErrorMessage = message, StatusCode = statusCode };
}
