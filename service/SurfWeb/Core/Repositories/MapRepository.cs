using Core.IRepository;
using Core.Models;

namespace Core.Repositories
{
    public class MapRepository : BaseRepository<MapModel>, IMapRepository
    {
        public MapRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}