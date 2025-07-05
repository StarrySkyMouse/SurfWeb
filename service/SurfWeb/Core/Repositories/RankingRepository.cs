using Core.IRepository;
using Core.Models;

namespace Core.Repositories
{
    public class RankingRepository : BaseRepository<RankingModel>, IRankingRepository
    {
        public RankingRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}