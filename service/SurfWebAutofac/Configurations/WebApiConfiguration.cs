using System.Reflection;
using Common.CustomMiddlewares.ApiResponse;
using Common.JsonConverters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Configurations;

/// <summary>
/// </summary>
public static class WebApiConfiguration
{
    /// <summary>
    ///     配置Web API相关服务
    /// </summary>
    public static void AddWebApiConfiguration(this WebApplicationBuilder builder)
    {
        //为每个请求生成唯一id
        builder.Services.AddHttpContextAccessor();
        // 注册Swagger
        builder.Services.AddEndpointsApiExplorer();
        //当swagger无法使用时请尝试清除浏览器缓存！！！！！！！！！！！！
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, true);
        });
        // 注册控制器和数据格式化
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTiemConverter());
                options.JsonSerializerOptions.Converters.Add(new FloatConverter());
                options.JsonSerializerOptions.Converters.Add(new DecimalConverter());
                options.JsonSerializerOptions.Converters.Add(new Int64ToStringConverter());
            });
        //注册CORS策略 
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    /// <summary>
    ///     使用Web API中间件
    /// </summary>
    /// <param name="app"></param>
    public static void UseAppMiddleware(this WebApplication app)
    {
        app.Services.CreateAsyncScope(); //启用AspectCore依赖注入
        // 调试环境下启用Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        //Cors中间件
        app.UseCors("AllowAllOrigins");
        //api响应中间件
        app.UseAuthorization();
        app.MapControllers();
        app.UseApiResponse();
    }
}