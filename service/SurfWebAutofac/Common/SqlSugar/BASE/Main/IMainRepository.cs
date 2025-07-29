namespace Common.SqlSugar.BASE.Main;

public interface IMainRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}