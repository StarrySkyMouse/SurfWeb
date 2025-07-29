using Common.SqlSugar.BASE.Log;
using IServices.Log;
using Model.Models.Log;
using Services.Base;

namespace Services.Log;

public class ServicesLogServices : BaseServices<ServicesLogModel>, IServicesLogServices
{
    public ServicesLogServices(ILogRepository<ServicesLogModel> dbLogRepository) : base(dbLogRepository)
    {
    }
}