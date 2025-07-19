using Model.Models.Base;
using SqlSugar;

namespace Model.Models
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
        /// 排名类型(积分、主线、奖励、阶段)
        /// </summary>
        [SugarColumn(ColumnDescription = "排名类型(积分、主线、奖励、阶段)")]
        public RankingTypeEnum Type { get; set; }

        /// <summary>
        /// 名次
        /// </summary>
        [SugarColumn(ColumnDescription = "名次")]
        public int Rank { get; set; }

        /// <summary>
        /// 玩家id
        /// </summary>
        [SugarColumn(ColumnDescription = "玩家id", IsNullable = false, Length = 64)]
        public required string PlayerId { get; set; }

        /// <summary>
        /// 玩家名
        /// </summary>
        [SugarColumn(ColumnDescription = "玩家名", IsNullable = false, Length = 128)]
        public required string PlayerName { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [SugarColumn(ColumnDescription = "数值")]
        public decimal Value { get; set; }
    }
}
