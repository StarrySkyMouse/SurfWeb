using Model.Models.Base;
using SqlSugar;

namespace Repository.BASE;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly ISqlSugarClient _sqlSugarClient;

    protected BaseRepository(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    ///     获取查询
    /// </summary>
    public ISugarQueryable<TEntity> Queryable()
    {
        return _sqlSugarClient.Queryable<TEntity>().Where(t => t.IsDelete == 0);
    }

    /// <summary>
    ///     新增
    /// </summary>
    public int Insert(TEntity insertObj)
    {
        return _sqlSugarClient.Insertable(insertObj).ExecuteCommand();
    }

    /// <summary>
    ///     批量新增
    /// </summary>
    public int Inserts(List<TEntity> insertObjs)
    {
        return _sqlSugarClient.Insertable(insertObjs).ExecuteCommand();
    }

    /// <summary>
    ///     批量更新
    /// </summary>
    public int Updates(List<TEntity> updateObjs)
    {
        return _sqlSugarClient.Updateable(updateObjs).ExecuteCommand();
    }

    /// <summary>
    ///     更新
    /// </summary>
    public int Update(TEntity updateObj)
    {
        return _sqlSugarClient.Updateable(updateObj).ExecuteCommand();
    }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public int Delete(long id)
    {
        return _sqlSugarClient.Updateable<TEntity>()
            .SetColumns(it =>
                new TEntity
                {
                    IsDelete = 1,
                    UpDateTime = DateTime.Now
                }).Where(t => t.Id == id).ExecuteCommand();
    }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public int Delete(TEntity deleteObj)
    {
        return _sqlSugarClient.Updateable<TEntity>()
            .SetColumns(it =>
                new TEntity
                {
                    IsDelete = 1,
                    UpDateTime = DateTime.Now
                }).Where(t => t.Id == deleteObj.Id).ExecuteCommand();
    }

    /// <summary>
    ///     批量逻辑删除
    /// </summary>
    public int Deletes(List<TEntity> deleteObjs)
    {
        return _sqlSugarClient.Updateable<TEntity>()
            .SetColumns(it =>
                new TEntity
                {
                    IsDelete = 1,
                    UpDateTime = DateTime.Now
                }).Where(t => deleteObjs.Select(a => a.Id).Contains(t.Id)).ExecuteCommand();
    }
}