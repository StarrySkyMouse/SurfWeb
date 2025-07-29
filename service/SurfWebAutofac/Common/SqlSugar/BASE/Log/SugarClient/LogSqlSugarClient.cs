using SqlSugar;

namespace Common.SqlSugar.BASE.Log.SugarClient;

/// <summary>
///     多ISqlSugarClient实例
/// </summary>
public class LogSqlSugarClient : SqlSugarClient, ILogSqlSugarClient
{
    public LogSqlSugarClient(ConnectionConfig config) : base(config)
    {
    }

    public LogSqlSugarClient(List<ConnectionConfig> configs) : base(configs)
    {
    }

    public LogSqlSugarClient(ConnectionConfig config, Action<SqlSugarClient> configAction) : base(config, configAction)
    {
    }

    public LogSqlSugarClient(List<ConnectionConfig> configs, Action<SqlSugarClient> configAction) : base(configs,
        configAction)
    {
    }
}