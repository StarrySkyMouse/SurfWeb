using Core.IRepository.Base;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Linq.Expressions;

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
        public List<TEntity> Inserts(IEnumerable<TEntity> entitys)
        {
            var result = new List<TEntity>();
            foreach (var entity in entitys)
            {
                result.Add(Insert(entity));
            }
            return result;
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
        public void Updates(IEnumerable<TEntity> entitys)
        {
            var list = _dbContext.Set<TEntity>().IgnoreQueryFilters().Where(t => t.IsDelete == 0 && entitys.Select(a => a.Id).Contains(t.Id)).ToList();
            foreach (var formerEntity in list)
            {
                var entity = entitys.First(t => t.Id == formerEntity.Id);
                foreach (var property in formerEntity.GetType().GetProperties())
                {
                    object value2 = property.GetValue(entity);
                    property.SetValue(formerEntity, value2);
                }
                formerEntity.UpDateTime = DateTime.Now;
            }
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
        public void DeleteAll()
        {
            var listAll = _dbContext.Set<TEntity>().ToList();
            if (listAll.Any())
            {
                listAll.ForEach(t =>
                {
                    t.UpDateTime = DateTime.Now;
                    t.IsDelete = 1;
                });
            }
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        /// <summary>
        /// 数据库事务
        /// </summary>
        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
        #endregion
    }
}