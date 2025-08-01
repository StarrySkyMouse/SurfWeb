using Model.Entitys;
using Repositories.Base;
using Repositories.IRepository;

namespace Repositories.Repository;

public class PlayerCompleteRepository : BaseRepository<PlayerCompleteModel>, IPlayerCompleteRepository
{
    public PlayerCompleteRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}