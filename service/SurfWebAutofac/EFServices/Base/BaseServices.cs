using Common.Db.Base;
using Common.Db.EFCore.Repository;
using IServices.Base;

namespace EFServices.Base;

/// <summary>
///     通用服务层实现
///     负责封装业务逻辑，将业务规则与数据访问、表示层解耦。
/// </summary>
public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity, new()
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseServices(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public long Insert(TEntity insertObj)
    {
        var result = _repository.Insert(insertObj);
        _repository.SaveChanges();
        return result.Id;
    }

    public long Inserts(List<TEntity> insertObjs)
    {
        var result = _repository.Inserts(insertObjs).Count;
        _repository.SaveChanges();
        return result;
    }

    public int Update(TEntity updateObj)
    {
        _repository.Update(updateObj);
        _repository.SaveChanges();
        return 1;
    }

    public int Updates(List<TEntity> updateObjs)
    {
        _repository.Updates(updateObjs);
        _repository.SaveChanges();
        return 1;
    }

    public int Delete(long id)
    {
        _repository.Delete(id);
        _repository.SaveChanges();
        return 1;
    }

    public int Delete(TEntity deleteObj)
    {
        _repository.Delete(deleteObj);
        _repository.SaveChanges();
        return 1;
    }

    public int Deletes(List<TEntity> deleteObjs)
    {
        _repository.Deletes(deleteObjs);
        _repository.SaveChanges();
        return deleteObjs.Count;
    }
}