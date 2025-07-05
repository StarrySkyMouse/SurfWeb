using Core.IRepository;
using Core.Models;

namespace Core.Repositories
{
    public class PlayerCompleteRepository : BaseRepository<PlayerCompleteModel>, IPlayerCompleteRepository
    {
        public PlayerCompleteRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}