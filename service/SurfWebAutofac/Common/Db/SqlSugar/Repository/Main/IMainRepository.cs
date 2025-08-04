using Common.Db.Base;

namespace Common.Db.SqlSugar.Repository.Main;

public interface IMainRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}