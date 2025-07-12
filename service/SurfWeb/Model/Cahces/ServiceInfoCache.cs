using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Cahces
{
    public class ServiceInfoCache
    {
        public string Map { get; set; }
        public ServiceInfoMapInfoCache? MapInfo { get; set; }
        public List<ServiceInfoPlayerInfoCache> PlayerInfos { get; set; }
        public int MaxPlayers { get; set; }
    }

    public class ServiceInfoMapInfoCache
    {
        public required string Id { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        public required string Difficulty { get; set; }
        /// <summary>
        /// 地图图片
        public required string Img { get; set; }
    }

    public class ServiceInfoPlayerInfoCache
    {
        public string Name { get; set; }
        public float Duration { get; set; } // 在线时长(秒)
    }
}
