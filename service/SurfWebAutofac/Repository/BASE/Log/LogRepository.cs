using Model.Models.Base;
using Repository.BASE.Log.SugarClient;
using Repository.BASE.MainSqlSugar;

namespace Repository.BASE.Log;

public class LogRepository<TEntity> : BaseRepository<TEntity>, IMainRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    private LogRepository(ILogSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}