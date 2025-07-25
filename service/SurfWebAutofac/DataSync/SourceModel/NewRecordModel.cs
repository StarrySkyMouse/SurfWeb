using System.ComponentModel.DataAnnotations;
using DataSync.SourceModel.Base;

namespace DataSync.SourceModel
{
    /// <summary>
    /// 纪录类型
    /// </summary>
    public enum RecordTypeEnum
    {
        /// <summary>
        /// 主线
        /// </summary>
        Main,
        /// <summary>
        /// 奖励
        /// </summary>
        Bounty,
        /// <summary>
        /// 阶段
        /// </summary>
        Stage
    }
    /// <summary>
    /// 最新记录
    /// </summary>
    public class NewRecordModel : BaseEntity
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        [MaxLength(64)]
        public required string PlayerId { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        [MaxLength(128)]
        public required string PlayerName { get; set; }
        /// <summary>
        /// 地图id
        /// </summary>
        [MaxLength(64)]
        public required string MapId { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        [MaxLength(128)]
        public required string MapName { get; set; }
        /// <summary>
        /// 通关类型(主线，奖励，阶段)
        /// </summary>
        [MaxLength(1)]
        public RecordTypeEnum Type { get; set; }
        /// <summary>
        /// 说明难度或阶段
        /// </summary>
        [MaxLength(64)]
        public required string Notes { get; set; }
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