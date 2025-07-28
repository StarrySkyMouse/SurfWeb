namespace Common.Logger;

public class LoggerConfig
{
    /// <summary>
    ///     文件配置
    /// </summary>
    public LoggerConfig_FileConfig FileConfig { get; set; }

    /// <summary>
    ///     Seq
    /// </summary>
    public LoggerConfig_Seq SeqConfig { get; set; }
}

public class LoggerConfig_FileConfig
{
    /// <summary>
    ///     路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    ///     最多保留多少天的日志文件
    /// </summary>
    public int RetentionDay { get; set; }
}

public class LoggerConfig_Seq
{
    /// <summary>
    ///     url
    /// </summary>
    public string Url { get; set; }
}