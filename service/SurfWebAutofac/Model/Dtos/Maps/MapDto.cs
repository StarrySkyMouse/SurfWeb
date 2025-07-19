namespace Model.Dtos.Maps
{
    public class MapDto
    {
        public required long id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
        /// <summary>
        /// 地图图片
        /// </summary>
        public required string Img { get; set; }
        /// <summary>
        /// 完成人数
        /// </summary>
        public int SurcessNumber { get; set; }
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
