using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.NewRecords
{
    public class PopularMapDto
    {
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
        /// <summary>
        /// 完成人数
        /// </summary>
        public int SurcessNumber { get; set; }
    }
}
