using Model.Entitys;

namespace Model.Dtos.NewRecords
{
    public class NewRecordDto
    {
        /// <summary>
        /// 地图id
        /// </summary>
        public  string MapId { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public  string MapName { get; set; }
        /// <summary>
        /// 通关类型(主线，奖励，阶段)
        /// </summary>
        public RecordTypeEnum Type { get; set; }
        /// <summary>
        /// 说明难度或阶段
        /// </summary>
        public  string Notes { get; set; }
        /// <summary>
        /// 玩家信息
        /// </summary>
        public List<NewRecordDto_Player> Players { get; set; }
    }

    public class NewRecordDto_Player
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public  string PlayerId { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public  string PlayerName { get; set; }
        /// <summary>
        /// 通关时间
        /// </summary>
        public float Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        //单独使用示例
        //[JsonConverter(typeof(DateTiemConverter))]
        public DateTime Date { get; set; }
    }

    public class NewRecordDtoOld
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
        /// 通关类型(主线，奖励，阶段)
        /// </summary>
        public RecordTypeEnum Type { get; set; }
        /// <summary>
        /// 说明难度或阶段
        /// </summary>
        public required string Notes { get; set; }
        /// <summary>
        /// 通关时间
        /// </summary>
        public float Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        //单独使用示例
        //[JsonConverter(typeof(DateTiemConverter))]
        public DateTime Date { get; set; }
    }
}
