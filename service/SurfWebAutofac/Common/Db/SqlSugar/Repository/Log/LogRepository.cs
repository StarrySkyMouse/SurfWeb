using Common.Db.Base;
using Common.Db.SqlSugar.Repository.Log.SugarClient;

namespace Common.Db.SqlSugar.Repository.Log;

public class LogRepository<TEntity> : BaseRepository<TEntity>, ILogRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public LogRepository(ILogSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}