using Core.IRepository;
using Core.Models;

namespace Core.Repositories
{
    public class NewRecordRepository : BaseRepository<NewRecordModel>, INewRecordRepository
    {
        public NewRecordRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}