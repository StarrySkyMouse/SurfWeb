using Model.Entitys;
using Repositories.Base;
using Repositories.IRepository;

namespace Repositories.Repository;

public class NewRecordRepository : BaseRepository<NewRecordModel>, INewRecordRepository
{
    public NewRecordRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}