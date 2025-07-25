using Model.Models.Base;

namespace Repository.BASE.Log;

public interface ILogRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}