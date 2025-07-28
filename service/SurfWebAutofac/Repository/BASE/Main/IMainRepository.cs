using Model.Models.Base;

namespace Repository.BASE.Main;

public interface IMainRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}