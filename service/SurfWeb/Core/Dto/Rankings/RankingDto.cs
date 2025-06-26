using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class RankingDto
    {
        /// <summary>
        /// 排名类型(积分)
        /// </summary>
        public RankingTypeEnum Type { get; set; }
        /// <summary>
        /// 名次
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 玩家id
        /// </summary>
        public required string PlayerId { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public required string PlayerName { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public string Progress { get; set; }
    }
}
