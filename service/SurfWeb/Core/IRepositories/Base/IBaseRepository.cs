using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.Base
{
    /// <summary>
    /// 通用仓储服务接口，定义了基础的数据操作方法。
    /// 适用于所有继承自 <see cref="BasicEntity"/> 的实体类型。
    /// </summary>
    public interface IBaseRepository<TEntity> : IQueryable<TEntity>
    {
        /// <summary>
        /// 新增
        /// </summary>
        TEntity Insert(TEntity entity);
        /// <summary>
        /// 修改
        /// </summary>
        void Update(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        void Delete(string Id);
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
