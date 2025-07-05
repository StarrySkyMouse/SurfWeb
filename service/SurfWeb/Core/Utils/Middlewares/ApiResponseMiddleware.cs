using Core.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Core.Utils.Middlewares
{
    public class ApiResponse<T>
    {
        public int code { get; set; } // 0:成功，其他:失败
        public string? message { get; set; }
        public T? data { get; set; }

        public static ApiResponse<T> Success(T? data, string message = "success") =>
            new ApiResponse<T> { code = 20000, message = message, data = data };

        public static ApiResponse<T> Fail(string message, int code) =>
            new ApiResponse<T> { code = code, message = message, data = default };
    }
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                // 只包装成功的JSON响应
                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300 &&
                    context.Response.ContentType != null &&
                    context.Response.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
                    //处理api返回null
                    || context.Response.StatusCode == 204)
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                    object? data = null;
                    if (!string.IsNullOrWhiteSpace(responseBody))
                    {
                        try
                        {
                            data = System.Text.Json.JsonSerializer.Deserialize<object>(responseBody);
                        }
                        catch
                        {
                            data = responseBody; // 不是标准json时直接返回原文
                        }
                    }
                    //防止204
                    context.Response.StatusCode = 200;
                    var result = ApiResponse<object>.Success(data);
                    var json = System.Text.Json.JsonSerializer.Serialize(result);

                    context.Response.ContentLength = Encoding.UTF8.GetByteCount(json);
                    context.Response.Body = originalBodyStream;
                    context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBodyStream);
                    context.Response.Body = originalBodyStream;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.Body = originalBodyStream;
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json; charset=utf-8";
                ApiResponse<object> result;
                if (ex is CustomException)
                {
                    result = ApiResponse<object>.Fail(ex.Message, ((CustomException)ex).ErrorCode);
                }
                else
                {
                    result = ApiResponse<object>.Fail("系统异常请联系管理员", 50001);
                }
                var json = System.Text.Json.JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(json);
            }
        }
    }
}