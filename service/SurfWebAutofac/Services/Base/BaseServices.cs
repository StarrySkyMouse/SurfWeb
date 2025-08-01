using Common.Db.Base;
using Common.Db.SqlSugar.Repository;
using IServices.Base;

namespace Services.Base;

public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity, new()
{
    private readonly IBaseRepository<TEntity> _baseRepository;

    public BaseServices(IBaseRepository<TEntity> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    /// <summary>
    ///     新增
    /// </summary>
    public long Insert(TEntity insertObj)
    {
        return _baseRepository.Insert(insertObj);
    }

    /// <summary>
    ///     批量新增
    /// </summary>
    public long Inserts(List<TEntity> insertObjs)
    {
        return _baseRepository.Inserts(insertObjs);
    }

    /// <summary>
    ///     批量更新
    /// </summary>
    public int Update(TEntity updateObj)
    {
        return _baseRepository.Update(updateObj);
    }

    /// <summary>
    ///     更新
    /// </summary>
    public int Updates(List<TEntity> updateObjs)
    {
        return _baseRepository.Updates(updateObjs);
    }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public int Delete(long id)
    {
        return _baseRepository.Delete(id);
    }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public int Delete(TEntity deleteObj)
    {
        return _baseRepository.Delete(deleteObj);
    }

    /// <summary>
    ///     批量逻辑删除
    /// </summary>
    public int Deletes(List<TEntity> deleteObjs)
    {
        return _baseRepository.Deletes(deleteObjs);
    }
}