namespace Core.Dto.Players
{
    /// <summary>
    /// 玩家数据同步传输对象
    /// </summary>
    public class DataSyncPlayerDto
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 腐竹玩家id
        /// </summary>
        public int Auth { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Integral { get; set; }
    }
}
