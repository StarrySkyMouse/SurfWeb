using Model.Entitys;
using Repositories.Base;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class MapRepository : BaseRepository<MapModel>, IMapRepository
    {
        public MapRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}