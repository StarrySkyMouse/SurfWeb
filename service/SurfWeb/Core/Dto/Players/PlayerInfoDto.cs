namespace Core.Dto.Players
{
    public class PlayerInfoDto
    {
        public required string Id { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 积分排行
        /// </summary>
        public int IntegralRanking { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Integral { get; set; }
        /// <summary>
        /// 地图完成排行
        /// </summary>
        public int SucceesRanking { get; set; }
        /// <summary>
        /// 地图完成数
        /// </summary>
        public int SucceesNumber { get; set; }
        /// <summary>
        /// 主线wr排行
        /// </summary>
        public int WRRanking { get; set; }
        /// <summary>
        /// 主线wr数
        /// </summary>
        public int WRNumber { get; set; }
        /// <summary>
        /// 奖励wr排行
        /// </summary>
        public int BWRanking { get; set; }
        /// <summary>
        /// 奖励wr数
        /// </summary>
        public int BWRNumber { get; set; }
        /// <summary>
        /// 阶段wr排行
        /// </summary>
        public int SWRanking { get; set; }
        /// <summary>
        /// 阶段wr数
        /// </summary>
        public int SWRNumber { get; set; }
    }
}
