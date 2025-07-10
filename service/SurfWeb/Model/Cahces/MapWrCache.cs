using Model.Entitys;

namespace Model.Cahces
{
    /// <summary>
    /// 地图wr缓存
    /// </summary>
    public class MapWrCache
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public required string PlayerId { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public required string PlayerName { get; set; }
        /// <summary>
        /// 地图id
        /// </summary>
        public required string MapId { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public required string MapName { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
        /// <summary>
        /// 通关类型(主线，奖励，阶段)
        /// </summary>
        public RecordTypeEnum Type { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public int? Stage { get; set; }
        /// <summary>
        /// 通关时间
        /// </summary>
        public float Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
