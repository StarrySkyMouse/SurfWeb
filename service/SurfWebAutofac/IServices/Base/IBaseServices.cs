using System.Data;
using System.Linq.Expressions;
using Model;
using Model.Models.Base;
using SqlSugar;

namespace IServices.Base
{
    public interface IBaseServices<TEntity> where TEntity : BaseEntity
    {
    }
}
