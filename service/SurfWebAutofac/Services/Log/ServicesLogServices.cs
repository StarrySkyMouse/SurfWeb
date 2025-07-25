using IServices.Log;
using Model.Models.Log;
using Repository.BASE.Log;
using Services.Base;

namespace Services.Log;

public class ServicesLogServices : BaseServices<ServicesLogModel>, IDbLogServices
{
    public ServicesLogServices(ILogRepository<ServicesLogModel> dbLogRepository) : base(dbLogRepository)
    {
    }
}