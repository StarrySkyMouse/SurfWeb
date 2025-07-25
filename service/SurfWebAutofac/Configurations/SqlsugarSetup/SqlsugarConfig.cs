using SqlSugar;

namespace Configurations.SqlsugarSetup;

/// <summary>
///     SqlSugar配置类
/// </summary>
public class SqlsugarConfig
{
    /// <summary>
    ///     数据库类型  MySql = 0,SqlServer = 1,Sqlite = 2,Oracle = 3,PostgreSQL = 4（更多请查看DbType枚举）
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    ///     是否开启数据库初始化
    /// </summary>
    public bool IsDataCreate { get; set; }

    /// <summary>
    ///     雪花算法配置类
    /// </summary>
    public SqlsugarConfig_SnowflakeIdConfig SnowflakeIdConfig { get; set; }

    /// <summary>
    ///     主库连接字符串
    /// </summary>
    public SqlsugarConfig_MainConfig MainConfig { get; set; }

    /// <summary>
    ///     从库连接配置
    /// </summary>
    public SqlsugarConfig_SlaveConfig SlaveConfig { get; set; }

    /// <summary>
    ///     日志库配置
    /// </summary>
    public SqlsugarConfig_LogConfig LogConfig { get; set; }
}

/// <summary>
///     雪花算法配置类
/// </summary>
public class SqlsugarConfig_SnowflakeIdConfig
{
    /// <summary>
    ///     工作机器ID
    /// </summary>
    public int WorkId { get; set; }

    /// <summary>
    ///     数据中心ID
    /// </summary>
    public int DatacenterId { get; set; }
}

/// <summary>
///     主库配置类
/// </summary>
public class SqlsugarConfig_MainConfig
{
    /// <summary>
    ///     连接字符串
    /// </summary>
    public string DbConnection { get; set; }

    /// <summary>
    ///     是否开启从库
    /// </summary>
    public bool IsOpenSlave { get; set; }

    /// <summary>
    ///     从库配置
    /// </summary>
    public List<SqlsugarConfig_SlaveConfig> SlaveConfigs { get; set; }
}

public class SqlsugarConfig_SlaveConfig
{
    /// <summary>
    ///     是否启用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    ///     连接字符串
    /// </summary>
    public string DbConnection { get; set; }

    /// <summary>
    ///     从库权重
    /// </summary>
    public int HitRate { get; set; }
}

/// <summary>
///     日志库配置
/// </summary>
public class SqlsugarConfig_LogConfig
{
    /// <summary>
    ///     连接字符串
    /// </summary>
    public string DbConnection { get; set; }
}