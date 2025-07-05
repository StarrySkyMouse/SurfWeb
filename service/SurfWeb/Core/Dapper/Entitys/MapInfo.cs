namespace Core.Dapper.Entitys
{
    public class MapInfo
    {
        public string map { get; set; }
        public int number { get; set; }
        /// <summary>
        /// 1-奖励，2-阶段
        /// </summary>
        public int type { get; set; }
    }
}
