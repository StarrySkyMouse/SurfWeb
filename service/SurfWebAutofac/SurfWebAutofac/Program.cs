using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Core;
using Extensions.Apollo;
using Extensions.ServiceExtensions;
using SurfWebAutofac.Filter;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacModuleRegister());
        builder.RegisterModule<AutofacPropertityModuleReg>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        hostingContext.Configuration.ConfigureApplication();
        config.Sources.Clear();
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        config.AddConfigurationApollo("appsettings.apollo.json");
    });
builder.ConfigureApplication();

//SqlSugar
builder.Services.AddSqlsugarSetup();