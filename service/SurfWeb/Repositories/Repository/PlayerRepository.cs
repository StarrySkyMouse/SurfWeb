using Model.Entitys;
using Repositories.Base;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class PlayerRepository : BaseRepository<PlayerModel>, IPlayerRepository
    {
        public PlayerRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}