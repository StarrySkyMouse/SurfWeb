using System.Data;
using System.Linq.Expressions;
using Model;
using Model.Models.Base;
using SqlSugar;

namespace Repository.BASE
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        ISugarQueryable<TEntity> Queryable();
    }
}