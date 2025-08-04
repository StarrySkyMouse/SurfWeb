using Microsoft.AspNetCore.Builder;

namespace Common.CustomMiddlewares.ApiResponse;

public static class WebApplicationExtensions
{
    /// <summary>
    ///     添加Api响应中间件
    /// </summary>
    /// <param name="app"></param>
    public static void UseApiResponse(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApiResponseMiddleware>();
    }
}