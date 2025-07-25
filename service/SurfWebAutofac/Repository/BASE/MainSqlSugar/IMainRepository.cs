using Model.Models.Base;

namespace Repository.BASE.MainSqlSugar;

public interface IMainRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}