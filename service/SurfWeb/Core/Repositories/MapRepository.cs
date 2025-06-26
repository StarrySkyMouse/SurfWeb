using Core.IRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class MapRepository : BaseRepository<MapModel>, IMapRepository
    {
        public MapRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}