using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Configurations.AutofacSetup.AOP;
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
        var repositoryAssembly = Assembly.LoadFrom(Path.Combine(basePath, "Repository.dll"));

        //AOP
        builder.RegisterType<LoggingInterceptor>();

        // 扫描并注册 Services 层所有实现 IServices 接口的类型
        builder.RegisterAssemblyTypes(servicesAssembly)
            //是啊下只注册实现了 IServices 接口的类型
            .Where(t => t.GetInterfaces().Any(i => i.Assembly == iservicesAssembly))
            //按接口注入
            .AsImplementedInterfaces()
            //设置生命周期为每请求一个实例（Scoped）
            .InstancePerLifetimeScope()
            //启用接口拦截器（AOP）。
            .EnableInterfaceInterceptors()
            //日志拦截器
            .InterceptedBy(typeof(LoggingInterceptor))
            //Service缓存拦截器
            .InterceptedBy(typeof(CacheInterceptor));
        // 扫描并注册 Repository 层所有类型
        builder.RegisterAssemblyTypes(repositoryAssembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(LoggingInterceptor));
    }
}