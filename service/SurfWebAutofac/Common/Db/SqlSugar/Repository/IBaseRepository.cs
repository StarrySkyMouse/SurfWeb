using Common.Db.Base;
using SqlSugar;

namespace Common.Db.SqlSugar.Repository;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    /// <summary>
    ///     获取查询
    /// </summary>
    ISugarQueryable<TEntity> Queryable();

    /// <summary>
    ///     获取更新
    /// </summary>
    /// <returns></returns>
    IUpdateable<TEntity> Updateable();

    /// <summary>
    ///     新增
    /// </summary>
    long Insert(TEntity insertObj);

    /// <summary>
    ///     批量新增
    /// </summary>
    long Inserts(List<TEntity> insertObjs);

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

    /// <summary>
    ///     逻辑删除全部
    /// </summary>
    void DeleteAll();
}