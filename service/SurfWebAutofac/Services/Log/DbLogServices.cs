using Common.Db.SqlSugar.Repository.Log;
using IServices.Log;
using Model.Models.Log;
using Services.Base;

namespace Services.Log;

public class DbLogServices : BaseServices<DbLogModel>, IDbLogServices
{
    public DbLogServices(ILogRepository<DbLogModel> dbLogRepository) : base(dbLogRepository)
    {
    }
}