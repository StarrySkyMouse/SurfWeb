using Model.Models.Base;
using Repository.BASE.Main.SugarClient;

namespace Repository.BASE.Main;

public class MainRepository<TEntity> : BaseRepository<TEntity>, IMainRepository<TEntity>
    where TEntity : BaseEntity, new()
{
    public MainRepository(IMainSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }
}