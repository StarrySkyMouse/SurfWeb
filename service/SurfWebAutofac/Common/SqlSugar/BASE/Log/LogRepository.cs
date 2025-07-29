using Common.SqlSugar.BASE.Log.SugarClient;

namespace Common.SqlSugar.BASE.Log;

public class LogRepository<TEntity> : BaseRepository<TEntity>, ILogRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public LogRepository(ILogSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}