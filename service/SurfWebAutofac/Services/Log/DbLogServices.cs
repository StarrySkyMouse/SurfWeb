using IServices.Log;
using Model.Models.Log;
using Repository.BASE.Log;
using Services.Base;

namespace Services.Log;

public class DbLogServices : BaseServices<DbLogModel>, IDbLogServices
{
    public DbLogServices(ILogRepository<DbLogModel> dbLogRepository) : base(dbLogRepository)
    {
    }
}