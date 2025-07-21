using Microsoft.EntityFrameworkCore.Storage;

namespace Repositories.Base
{
    /// <summary>
    /// 通用仓储服务接口，定义了基础的数据操作方法。
    /// 适用于所有继承自 <see cref="BasicEntity"/> 的实体类型。
    /// </summary>
    public interface IBaseRepository<TEntity> : IQueryable<TEntity>
    {

    }
}
