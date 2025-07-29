using SqlSugar;

namespace Common.SqlSugar.BASE.Main.SugarClient;

/// <summary>
///     多ISqlSugarClient实例
/// </summary>
public class MainSqlSugarClient : SqlSugarClient, IMainSqlSugarClient
{
    public MainSqlSugarClient(ConnectionConfig config) : base(config)
    {
    }

    public MainSqlSugarClient(List<ConnectionConfig> configs) : base(configs)
    {
    }

    public MainSqlSugarClient(ConnectionConfig config, Action<SqlSugarClient> configAction) : base(config, configAction)
    {
    }

    public MainSqlSugarClient(List<ConnectionConfig> configs, Action<SqlSugarClient> configAction) : base(configs,
        configAction)
    {
    }
}