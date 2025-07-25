namespace Common.Quartz;

public class JobConfig
{
    /// <summary>
    ///     同步任务执行
    /// </summary>
    public List<JobConfig_SequenceJobConfig> SequenceJobConfigs { get; set; } = new();

    public List<JobConfig_JobConfig> JobConfigs { get; set; } = new();
}

/// <summary>
///     简单同步任务
/// </summary>
public class JobConfig_SequenceJobConfig
{
    /// <summary>
    ///     名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     间隔多少分钟执行一次
    /// </summary>
    public int SecondMinute { get; set; }
}

/// <summary>
///     简单定时任务
/// </summary>
public class JobConfig_JobConfig
{
    /// <summary>
    ///     名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     间隔多少分钟执行一次
    /// </summary>
    public int SecondMinute { get; set; }
}