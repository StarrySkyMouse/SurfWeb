using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class PlayerModel : BaseEntity
    {
        /// <summary>
        /// 腐竹玩家id
        /// </summary>
        public int Auth { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        [MaxLength(128)]
        public required string Name { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Integral { get; set; }
        /// <summary>
        /// 地图完成数
        /// </summary>
        public int SucceesNumber { get; set; }
        /// <summary>
        /// 主线wr数
        /// </summary>
        public int WRNumber { get; set; }
        /// <summary>
        /// 奖励wr数
        /// </summary>
        public int BWRNumber { get; set; }
        /// <summary>
        /// 阶段wr数
        /// </summary>
        public int SWRNumber { get; set; }
    }
}
