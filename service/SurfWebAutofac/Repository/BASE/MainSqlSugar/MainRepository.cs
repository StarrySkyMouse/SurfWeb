using Model.Models.Base;
using Repository.BASE.MainSqlSugar.SugarClient;

namespace Repository.BASE.MainSqlSugar;

public class MainRepository<TEntity> : BaseRepository<TEntity>, IMainRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    private MainRepository(IMainSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}