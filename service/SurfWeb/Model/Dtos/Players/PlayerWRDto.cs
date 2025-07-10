namespace Model.Dtos.Players
{
    public class PlayerWRDto
    {
        /// <summary>
        /// 地图Id
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
        /// 阶段
        /// </summary>
        public int? Stage { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public float Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
