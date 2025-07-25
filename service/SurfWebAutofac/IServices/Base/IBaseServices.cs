using Model.Models.Base;

namespace IServices.Base;

public interface IBaseServices<TEntity> where TEntity : BaseEntity, new()
{
    /// <summary>
    ///     新增
    /// </summary>
    int Insert(TEntity insertObj);

    /// <summary>
    ///     批量新增
    /// </summary>
    int Inserts(List<TEntity> insertObjs);

    /// <summary>
    ///     批量更新
    /// </summary>
    int Update(TEntity updateObj);

    /// <summary>
    ///     更新
    /// </summary>
    int Updates(List<TEntity> updateObjs);

    /// <summary>
    ///     逻辑删除
    /// </summary>
    int Delete(long id);

    /// <summary>
    ///     逻辑删除
    /// </summary>
    int Delete(TEntity deleteObj);

    /// <summary>
    ///     批量逻辑删除
    /// </summary>
    int Deletes(List<TEntity> deleteObjs);
}