namespace Model.Dtos.Maps
{
    public class MapListDto
    {
        /// <summary>
        /// 地图ID
        /// </summary>
        public required string Id { get; set; }
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
    }
}
