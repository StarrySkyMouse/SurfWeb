using IServices.Base;
using Model.Models.Base;
using Repository.BASE;

namespace Services.Base;

public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity, new()
{
    private readonly IBaseRepository<TEntity> _baseRepository;

    public BaseServices(IBaseRepository<TEntity> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    /// <summary>
    ///     无参构造方法不可用
    /// </summary>
    public BaseServices()
    {
    }

    /// <summary>
    ///     新增
    /// </summary>
    public int Insert(TEntity insertObj)
    {
        return _baseRepository.Insert(insertObj);
    }

    /// <summary>
    ///     批量新增
    /// </summary>
    public int Inserts(List<TEntity> insertObjs)
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