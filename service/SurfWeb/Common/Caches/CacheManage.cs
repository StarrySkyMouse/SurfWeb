using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Cahces;
using Services.IServices;
using Utils.Steam;

namespace Common.Caches
{
    public class CacheManage
    {
        private readonly IMapServices _mapServices;
        private readonly DataCache _dataCache;
        public CacheManage(IMapServices mapServices
        , DataCache dataCache)
        {
            _mapServices = mapServices;
            _dataCache = dataCache;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        public async Task UpdateMapCache()
        {
            var mapList = await _mapServices.GetMapCacheList();
            var mainList = mapList
                .Select(m => new MapMainCache { Id = m.Id, Name = m.Name, Difficulty = m.Difficulty })
                .ToImmutableList();
            var bountyList = mapList
                .Where(t => t.BonusNumber != 0)
                .SelectMany(t =>
                    Enumerable.Range(1, t.BonusNumber)
                        .Select(b => new MapBountyOrStageCache
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Difficulty = t.Difficulty,
                            Stage = b
                        }))
                .ToImmutableList();
            var stageList = mapList
                .Where(t => t.StageNumber != 0)
                .SelectMany(t =>
                    Enumerable.Range(1, t.StageNumber)
                        .Select(b => new MapBountyOrStageCache
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Difficulty = t.Difficulty,
                            Stage = b
                        }))
                .ToImmutableList();
            var mapWrList = (await _mapServices.GetMapWrCacheList()).ToImmutableList();
            var newSnapshot = new DataCache.MapCacheSnapshot(mainList, bountyList, stageList, mapWrList);
            _dataCache.SetCacheSnapshot(newSnapshot);
        }
        public async Task UpdateServiceInfoCache()
        {
            var serverInfo = SteamUtil.GetServerInfo("124.223.198.48", 27070);
            var playerListInfo = SteamUtil.GetServerPlayerList("124.223.198.48", 27070);
            var mapInfo = await _mapServices.GetMapInfoByName(serverInfo.Map);
            _dataCache.SetServiceInfoSnapshot(new ServiceInfoCache()
            {
                Map = serverInfo.Map,
                MapInfo = mapInfo == null ? null : new ServiceInfoMapInfoCache()
                {
                    Id = mapInfo.Id,
                    Difficulty = mapInfo.Difficulty,
                    Img = mapInfo.Img
                },
                MaxPlayers = 9,
                PlayerInfos = playerListInfo.Select(t=>new ServiceInfoPlayerInfoCache()
                {
                    Name = t.Name,
                    Duration = t.Duration
                }).ToList()
            });
        }
    }
}
