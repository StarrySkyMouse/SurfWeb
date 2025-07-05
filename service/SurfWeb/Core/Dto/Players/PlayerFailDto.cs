namespace Core.Dto.Players
{
    public class PlayerFailDto
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
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public int? Stage { get; set; }
    }
}
