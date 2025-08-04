using Autofac;
using Common.Autofac;
using Configurations.AutofacSetup.Register;
using IServices.Main.Base;
using Microsoft.AspNetCore.Builder;

namespace Configurations.AutofacSetup;

public static class AutofacConfiguration
{
    public static void AddSqlSugarAutofacConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddAutofacService(cfg =>
        {
            //配置程序集
            cfg.ServicesDll = "Services.dll";
            //为MainService注册拦截器
            cfg.ServicesType = typeof(IMainBaseServices<>);
        }, containerBuilderAction => { containerBuilderAction.RegisterModule<AutofacModuleRegister>(); });
    }

    public static void AddEFCoreAutofacConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddAutofacService(cfg =>
        {
            //配置程序集
            cfg.ServicesDll = "EFServices.dll";
            //关闭拦截器
            cfg.IsOpenInterceptor = false;
        });
    }
}