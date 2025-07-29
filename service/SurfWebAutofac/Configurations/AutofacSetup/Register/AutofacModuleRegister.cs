using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Common.Caches.AOP;
using Common.Logger.AOP;
using Common.SqlSugar.BASE.Log;
using Common.SqlSugar.BASE.Main;
using IServices.Main.Base;
using Module = Autofac.Module;

namespace Configurations.AutofacSetup.Register;

public class AutofacModuleRegister : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // 获取 bin 目录路径
        var basePath = AppContext.BaseDirectory;
        // 加载相关程序集
        var iservicesAssembly = Assembly.LoadFrom(Path.Combine(basePath, "IServices.dll"));
        var servicesAssembly = Assembly.LoadFrom(Path.Combine(basePath, "Services.dll"));
        var commonAssembly = Assembly.LoadFrom(Path.Combine(basePath, "Common.dll"));
        // 注册拦截器
        builder.RegisterAssemblyTypes(commonAssembly)
            .Where(t => typeof(IInterceptor).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
            .AsSelf()
            .SingleInstance(); // 单例
        // 扫描并注册 Services 层所有实现 IServices 接口的类型
        builder.RegisterAssemblyTypes(servicesAssembly)
            //只注册实现了 IServices 接口的类型
            .Where(t => t.GetInterfaces().Any(i => i.Assembly == iservicesAssembly))
            //接口注入
            .AsImplementedInterfaces()
            //属性注入
            .PropertiesAutowired()
            //设置生命周期为每请求一个实例（Scoped）
            .InstancePerLifetimeScope();
        //为MainService注册拦截器
        builder.RegisterAssemblyTypes(servicesAssembly)
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMainBaseServices<>)))
            .AsImplementedInterfaces()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ServiceLoggingInterceptor), typeof(CacheInterceptor))
            .PropertiesAutowired()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(MainRepository<>))
            .As(typeof(IMainRepository<>))
            .InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(LogRepository<>))
            .As(typeof(ILogRepository<>))
            .InstancePerLifetimeScope();
    }
}