using Microsoft.AspNetCore.Builder;

namespace Common.CustomMiddlewares.UniqueVisitor;

public static class WebApplicationExtensions
{
    /// <summary>
    ///     添加Api响应中间件
    /// </summary>
    /// <param name="app"></param>
    public static void UseUniqueVisitor(this IApplicationBuilder app)
    {
        app.UseMiddleware<UniqueVisitorMiddleware>();
    }
}