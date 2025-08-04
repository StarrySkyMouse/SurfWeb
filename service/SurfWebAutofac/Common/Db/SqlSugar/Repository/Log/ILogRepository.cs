using Common.Db.Base;

namespace Common.Db.SqlSugar.Repository.Log;

public interface ILogRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}