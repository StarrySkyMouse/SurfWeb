using Core.IRepository;
using Core.IRepository.Base;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Repositories
{
    /// <summary>
    /// 通用仓储服务实现，定义了基础的数据操作方法。
    /// 适用于所有继承自 <see cref="BaseEntity"/> 的实体类型。
    /// </summary>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;
        public BaseRepository(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<SurfWebDbContext>();
        }
        #region IQueryable实现
        public DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
        public Type ElementType => GetQueryable().ElementType;
        public Expression Expression => GetQueryable().Expression;
        public IQueryProvider Provider => GetQueryable().Provider;
        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsNoTracking();
        }
        #endregion

        #region IRepository实现
        /// <summary>
        /// 新增
        /// </summary>
        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("[新增]入参为空");
            }
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }
            entity.CreateTime = DateTime.Now;
            entity.UpDateTime = DateTime.Now;
            entity.IsDelete = 0;
            _dbContext.Set<TEntity>().Add(entity);
            return entity;
        }
        /// <summary>
        /// 修改
        /// </summary>
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("[修改]入参为空");
            }
            //跟踪查询
            var formerEntity = _dbContext.Set<TEntity>().FirstOrDefault(t => t.Id == entity.Id);
            if (formerEntity == null)
            {
                throw new Exception($"[修改]无ID为{entity.Id}的数据");
            }
            //修改数据
            foreach (var property in formerEntity.GetType().GetProperties())
            {
                object value2 = property.GetValue(entity);
                property.SetValue(formerEntity, value2);
            }
            formerEntity.UpDateTime = DateTime.Now;
        }
        /// <summary>
        /// 标记删除
        /// </summary>
        public void Delete(string id)
        {
            //跟踪查询
            var formerEntity = _dbContext.Set<TEntity>().FirstOrDefault(t => t.Id == id);
            if (formerEntity == null)
            {
                throw new Exception($"[删除]无ID为{id}的数据");
            }
            formerEntity.UpDateTime = DateTime.Now;
            formerEntity.IsDelete = 1;

        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        #endregion
    }
}