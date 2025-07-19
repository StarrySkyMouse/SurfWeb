using Model.Models.Base;
using SqlSugar;

namespace Repository.BASE
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }
        public ISugarQueryable<TEntity> Queryable()
        {
            return _sqlSugarClient.Queryable<TEntity>();
        }
    }
}