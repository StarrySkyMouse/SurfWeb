namespace Model.Dtos.Maps
{
    public class MapCacheDto
    {
        /// <summary>
        /// 地图ID
        /// </summary>
        public required string Id { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
        /// <summary>
        /// 地图奖励关数量
        /// </summary>
        public int BonusNumber { get; set; }
        /// <summary>
        /// 地图阶段数量
        /// </summary>
        public int StageNumber { get; set; }

    }
}
