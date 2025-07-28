using Model.Models.Base;
using Repository.BASE.Log.SugarClient;
using Repository.BASE.Main;

namespace Repository.BASE.Log;

public class LogRepository<TEntity> : BaseRepository<TEntity>, ILogRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public LogRepository(ILogSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}