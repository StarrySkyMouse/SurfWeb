using System.Data;
using System.Linq.Expressions;
using Common.Helper;
using IServices.Base;
using Model;
using Model.Models.Base;
using Repository.BASE;
using SqlSugar;

namespace Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
    {
        protected readonly IBaseRepository<TEntity> _tEntityEntityRepository;
        public BaseServices(IBaseRepository<TEntity> tEntityEntityRepository)
        {
            _tEntityEntityRepository = tEntityEntityRepository;
        }
    }
}