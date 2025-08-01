﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Configurations.AutofacSetup.Register;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Configurations.AutofacSetup;

public static class AutofacConfiguration
{
    public static void AddAutofacConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule<AutofacModuleRegister>();
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
}