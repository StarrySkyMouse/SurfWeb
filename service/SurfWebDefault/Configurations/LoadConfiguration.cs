namespace Configurations;

/// <summary>
/// </summary>
public static class LoadConfiguration
{
    /// <summary>
    ///     加载配置文件
    /// </summary>
    public static void AddLoadConfiguration(this WebApplicationBuilder builder)
    {
        // 先清空默认配置源
        builder.Configuration.Sources.Clear();
        // 添加主配置文件
        builder.Configuration.AddJsonFile("appsettings.json", false, true);
        // 获取环境变量
        var env = builder.Environment.EnvironmentName;
        // 判断 Development 配置文件是否存在
        var devConfigFile = $"appsettings.{env}.json";
        var openSourceConfigFile = "appsettings.OpenSource.json";
        if (env == "Development" && !File.Exists(devConfigFile) && File.Exists(openSourceConfigFile))
            // 如果是开发环境且没有 appsettings.Development.json，但有 OpenSource 配置，则加载 OpenSource
            builder.Configuration.AddJsonFile(openSourceConfigFile, false, true);
        else
            // 正常加载当前环境配置文件
            builder.Configuration.AddJsonFile(devConfigFile, true, true);
        builder.Configuration.AddEnvironmentVariables();
    }
}