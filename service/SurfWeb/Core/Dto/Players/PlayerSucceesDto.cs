namespace Core.Dto.Players
{
    public class PlayerSucceesDto
    {
        /// <summary>
        /// 地图id
        /// </summary>
        public required string MapId { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public required string MapName { get; set; }
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
        /// <summary>
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
    }
}
