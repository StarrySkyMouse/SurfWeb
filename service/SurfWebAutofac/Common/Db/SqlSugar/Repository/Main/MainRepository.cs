using Common.Db.Base;
using Common.Db.SqlSugar.Repository.Main.SugarClient;

namespace Common.Db.SqlSugar.Repository.Main;

public class MainRepository<TEntity> : BaseRepository<TEntity>, IMainRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public MainRepository(IMainSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}