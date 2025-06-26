using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dto.Maps
{
    public class MapDto
    {
        public required string Id { get; set; }
        /// <summary>
        /// WR玩家ID
        /// </summary>
        public string? WRPlayerId { get; set; }
        /// <summary>
        /// WR玩家名
        /// </summary>
        public string? WRPlayerName { get; set; }
        /// <summary>
        /// WR时间
        /// </summary>
        public TimeSpan? WRTime { get; set; }
        /// <summary>
        /// WR日期
        /// </summary>
        public DateTime? WRDate { get; set; }
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
