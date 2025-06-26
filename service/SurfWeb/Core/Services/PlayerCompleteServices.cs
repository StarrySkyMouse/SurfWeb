using Core.IRepository.Base;
using Core.IServices;
using Core.Models;
using Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PlayerCompleteServices : BaseServices<PlayerCompleteModel>, IPlayerCompleteServices
    {
        public PlayerCompleteServices(IBaseRepository<PlayerCompleteModel> repository) : base(repository)
        {
        }
    }
}
