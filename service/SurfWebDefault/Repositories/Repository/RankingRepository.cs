using Model.Entitys;
using Repositories.Base;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class RankingRepository : BaseRepository<RankingModel>, IRankingRepository
    {
        public RankingRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}