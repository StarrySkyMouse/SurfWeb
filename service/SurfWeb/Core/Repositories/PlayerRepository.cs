using Core.IRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class PlayerRepository : BaseRepository<PlayerModel>, IPlayerRepository
    {
        public PlayerRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}