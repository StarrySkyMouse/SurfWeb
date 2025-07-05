using Core.Models;

namespace Core.IServices.Base
{
    /// <summary>
    /// 通用服务层接口
    /// 负责封装业务逻辑，将业务规则与数据访问、表示层解耦。
    /// </summary>
    public interface IBaseServices<TEntity> where TEntity : BaseEntity
    {
        TEntity? GetById(string id, Func<TEntity, TEntity>? select = null);
        string Insert(TEntity entity);
        IEnumerable<string> Inserts(IEnumerable<TEntity> entities);
        void UpDate(TEntity entity);
        void Updates(IEnumerable<TEntity> entities);
        void Delete(string id);
    }
}
