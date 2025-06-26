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
    /// 最新记录仓储接口
    /// </summary>
    public interface INewRecordRepository : IBaseRepository<NewRecordModel>
    {
    }
}
