using SqlSugar;

namespace Configurations.SqlsugarSetup;

/// <summary>
///     SqlSugar配置类
/// </summary>
public class SqlsugarConfig
{
    /// <summary>
    ///     数据库类型
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    ///     主库连接字符串
    /// </summary>
    public string MasterConn { get; set; }

    /// <summary>
    /// 是否开启数据库初始化
    /// </summary>
    public bool IsDataCreate { get; set; }

    /// <summary>
    /// 是否开启从库
    /// </summary>
    public bool IsOpenSlave { get; set; }

    /// <summary>
    ///     从库连接配置
    /// </summary>
    public List<SqlsugarConfig_SlaveConn> SlaveConns { get; set; }
}
public class SqlsugarConfig_SlaveConn
{
    /// <summary>
    ///     从库连接字符串
    /// </summary>
    public string SlaveConn { get; set; }

    /// <summary>
    ///     从库权重
    /// </summary>
    public int HitRate { get; set; }
}