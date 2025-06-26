using Core.Dto;
using Core.IServices.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IRankingServices : IBaseServices<RankingModel>
    {
        Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType);
    }
}
