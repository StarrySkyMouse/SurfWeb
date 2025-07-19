using System.Reflection;

namespace Configurations;

/// <summary>
/// </summary>
public static class BusinessIocConfiguration
{
    /// <summary>
    ///     业务模块IOC注册配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddBusinessIocConfiguration(this WebApplicationBuilder builder)
    {
        //通过扫描的方式注册
        builder.Services.Scan(scan => scan
            //加载名为 "Core" 的程序集（DLL）    
            .FromAssemblies(Assembly.Load("Repositories"), Assembly.Load("Services"))
            //选择类名以 "Repository" 或 "Services" 结尾的类型
            .AddClasses(classes => classes.Where(type =>
                type.Name.EndsWith("Repository") || type.Name.EndsWith("Services")))
            //把这些类注册为它们实现的接口
            .AsImplementedInterfaces()
            //注册为作用域生命周期（Scoped）
            .WithScopedLifetime()
        );
    }
}