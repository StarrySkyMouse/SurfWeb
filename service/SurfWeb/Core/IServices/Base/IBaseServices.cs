using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void UpDate(TEntity entity);
        void Delete(string id);
    }
}
