using Model.Models.Base;
using SqlSugar;

namespace Repository.BASE;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly ISqlSugarClient _sqlSugarClient;

    private BaseRepository(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
    }
    public ISugarQueryable<TEntity> Queryable()
    {
        return _sqlSugarClient.Queryable<TEntity>();
    }

    IUpdateable<TEntity> IBaseRepository<TEntity>.Updateable(TEntity updateObj)
    {
        throw new NotImplementedException();
    }

    public int Updateable(TEntity updateObj)
    {
        return _sqlSugarClient.Updateable(updateObj).ExecuteCommand();
    }
}