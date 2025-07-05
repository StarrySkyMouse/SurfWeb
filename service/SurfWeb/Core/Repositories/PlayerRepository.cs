using Core.IRepository;
using Core.Models;

namespace Core.Repositories
{
    public class PlayerRepository : BaseRepository<PlayerModel>, IPlayerRepository
    {
        public PlayerRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}