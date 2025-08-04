using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Autofac.Register;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Common.Autofac;

public static class ServiceCollectionExtensions
{
    public static void AddAutofacService(this WebApplicationBuilder builder,
        Action<AutofacAssemblyCfgParam> autofacAssemblyCfg,
        Action<ContainerBuilder>? containerBuilderAction = null)
    {
        var cfg = new AutofacAssemblyCfgParam();
        autofacAssemblyCfg(cfg);
        AutofacAssemblyCfg.IServicesDll = cfg.IServicesDll;
        AutofacAssemblyCfg.ServicesDll = cfg.ServicesDll;
        AutofacAssemblyCfg.ServicesType = cfg.ServicesType;
        AutofacAssemblyCfg.IsOpenInterceptor = cfg.IsOpenInterceptor;
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule<DefaultModuleRegister>();
                containerBuilderAction?.Invoke(containerBuilder);
            })
            .ConfigureAppConfiguration(config =>
            {
                // 先清空默认配置源
                config.Sources.Clear();
                // 添加主配置文件
                config.AddJsonFile("appsettings.json", false, true);
                // 获取环境变量
                var env = builder.Environment.EnvironmentName;
                // 判断 Development 配置文件是否存在
                var devConfigFile = $"appsettings.{env}.json";
                var openSourceConfigFile = "appsettings.OpenSource.json";
                if (env == "Development" && !File.Exists(devConfigFile) && File.Exists(openSourceConfigFile))
                    // 如果是开发环境且没有 appsettings.Development.json，但有 OpenSource 配置，则加载 OpenSource
                    config.AddJsonFile(openSourceConfigFile, false, true);
                else
                    // 正常加载当前环境配置文件
                    config.AddJsonFile(devConfigFile, true, true);
                config.AddEnvironmentVariables();
            });
    }

    public class AutofacAssemblyCfgParam
    {
        /// <summary>
        ///     配置程序集名称
        /// </summary>
        public string IServicesDll { get; set; } = "IServices.dll";

        public string ServicesDll { get; set; }
        public Type ServicesType { get; set; }

        /// <summary>
        ///     是否开启拦截器
        /// </summary>
        public bool IsOpenInterceptor { get; set; } = true;
    }

    /// <summary>
    ///     配置程序集名称
    /// </summary>
    internal static class AutofacAssemblyCfg
    {
        public static string IServicesDll { get; set; }
        public static string ServicesDll { get; set; }
        public static Type ServicesType { get; set; }

        /// <summary>
        ///     是否开启拦截器
        /// </summary>
        public static bool IsOpenInterceptor { get; set; }
    }
}