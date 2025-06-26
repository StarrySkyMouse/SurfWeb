using Core.IRepository.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    /// <summary>
    /// 玩家通关仓储接口
    /// </summary>
    public interface IPlayerCompleteRepository : IBaseRepository<PlayerCompleteModel>
    {
    }
}
