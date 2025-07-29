using Common.SqlSugar.BASE.Main.SugarClient;

namespace Common.SqlSugar.BASE.Main;

public class MainRepository<TEntity> : BaseRepository<TEntity>, IMainRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public MainRepository(IMainSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}