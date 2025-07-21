using Model.Models.Base;
using SqlSugar;

namespace Repository.BASE;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    ISugarQueryable<TEntity> Queryable();
    IUpdateable<TEntity> Updateable(TEntity updateObj);
}