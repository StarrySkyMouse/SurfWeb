namespace Common.Middlewares.ApiResponse;

public class ApiResponse<T>
{
    public int code { get; set; } // 0:成功，其他:失败
    public string? message { get; set; }
    public T? data { get; set; }

    public static ApiResponse<T> Success(T? data, string message = "success")
    {
        return new ApiResponse<T> { code = 20000, message = message, data = data };
    }

    public static ApiResponse<T> Fail(string message, int code)
    {
        return new ApiResponse<T> { code = code, message = message, data = default };
    }
}