using Model.Entitys.Base;
using Repositories.Base;

namespace Services.Base;

/// <summary>
///     通用服务层实现
///     负责封装业务逻辑，将业务规则与数据访问、表示层解耦。
/// </summary>
public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseServices(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public TEntity? GetById(string id, Func<TEntity, TEntity>? select = null)
    {
        var query = _repository.AsQueryable();
        query = query.Where(t => t.Id == id);
        if (select != null) query = query.Select(select).AsQueryable();
        var result = query.FirstOrDefault();
        return result;
    }

    public string Insert(TEntity entity)
    {
        var result = _repository.Insert(entity);
        _repository.SaveChanges();
        return result.Id;
    }

    public IEnumerable<string> Inserts(IEnumerable<TEntity> entities)
    {
        _repository.Inserts(entities);
        _repository.SaveChanges();
        return entities.Select(e => e.Id);
    }

    public void UpDate(TEntity entity)
    {
        _repository.Update(entity);
        _repository.SaveChanges();
    }

    public void Updates(IEnumerable<TEntity> entities)
    {
        _repository.Updates(entities);
        _repository.SaveChanges();
    }

    public void Delete(string id)
    {
        _repository.Delete(id);
        _repository.SaveChanges();
    }
}