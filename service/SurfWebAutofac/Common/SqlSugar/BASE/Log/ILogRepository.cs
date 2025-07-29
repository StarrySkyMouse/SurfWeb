namespace Common.SqlSugar.BASE.Log;

public interface ILogRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
}