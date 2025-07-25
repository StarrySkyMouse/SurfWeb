namespace Common.Logger;

public class LoggerConfig
{
    /// <summary>
    ///     是否打开控制台日志
    /// </summary>
    public bool IsOpenConsole { get; set; } = false;

    /// <summary>
    ///     Db日志配置
    /// </summary>
    public bool IsOpenDb { get; set; } = false;

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
    ///     是否打开
    /// </summary>
    public bool IsOpen { get; set; } = false;

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
    ///     是否打开
    /// </summary>
    public bool IsOpen { get; set; } = false;

    /// <summary>
    ///     url
    /// </summary>
    public string Url { get; set; }
}