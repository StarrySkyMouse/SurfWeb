using Core.IRepository;
using Core.IRepository.Base;
using Core.IServices.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Base
{
    /// <summary>
    /// 通用服务层实现
    /// 负责封装业务逻辑，将业务规则与数据访问、表示层解耦。
    /// </summary>
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
    {
        private readonly IBaseRepository<TEntity> _repository;
        public BaseServices(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public TEntity? GetById(string id, Func<TEntity, TEntity>? select = null)
        {
            IQueryable<TEntity> query = _repository.AsQueryable();
            query = query.Where(t => t.Id == id);
            if (select != null)
            {
                query = query.Select(select).AsQueryable();
            }
            var result = query.FirstOrDefault();
            return result;
        }
        public string Insert(TEntity entity)
        {
            var result = _repository.Insert(entity);
            _repository.SaveChanges();
            return result.Id;
        }
        public void UpDate(TEntity entity)
        {
            _repository.Update(entity);
            _repository.SaveChanges();
        }
        public void Delete(string id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }
    }
}