using Model.Entitys.Base;

namespace Model.Entitys
{
    public enum RankingTypeEnum
    {
        /// <summary>
        /// 积分
        /// </summary>
        Integral,
        /// <summary>
        /// 主线
        /// </summary>
        MainWR,
        /// <summary>
        /// 奖励
        /// </summary>
        BountyWR,
        /// <summary>
        /// 阶段
        /// </summary>
        StageWR
    }
    /// <summary>
    /// 排行
    /// </summary>
    public class RankingModel : BaseEntity
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
        /// 数值
        /// </summary>
        public decimal Value { get; set; }
    }
}
