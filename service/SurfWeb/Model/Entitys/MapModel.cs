using System.ComponentModel.DataAnnotations;
using Model.Entitys.Base;

namespace Model.Entitys
{
    /// <summary>
    /// 地图
    /// </summary>
    public class MapModel : BaseEntity
    {
        [MaxLength(128)]
        public required string Name { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        [MaxLength(16)]
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
