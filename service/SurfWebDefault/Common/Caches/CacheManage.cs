using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Cahces;
using Services.IServices;
using Utils.Mail;
using Utils.Steam;

namespace Common.Caches
{
    public class CacheManage
    {
        private readonly IMapServices _mapServices;
        private readonly DataCache _dataCache;
        private readonly IPlayerServices _playerServices;
        public CacheManage(IMapServices mapServices
        , DataCache dataCache,
        IPlayerServices playerServices)
        {
            _mapServices = mapServices;
            _dataCache = dataCache;
            _playerServices = playerServices;
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
            //获取新加入的玩家
            var addList = playerListInfo.Where(t => !_dataCache.OldPlayerInfoList.Contains(t.Name));
            //获取离开的玩家
            var deleteList= _dataCache.OldPlayerInfoList.Where(t => !playerListInfo.Select(a => a.Name).Contains(t));
            //获取要监控的玩家
            var playeInfo= _playerServices.GetById("42bed37b-1f26-4903-851e-658549e8649d");
            if (playeInfo != null)
            {
                //进入游戏
                if (addList.Any(t => t.Name == playeInfo.Name))
                {
                    MailUtil.SendMessage("1422323984@qq.com", "1422323984@qq.com", "ffkmyvqapzyfbafb", "滑翔服玩家提示",$"玩家{playeInfo.Name}正在地狱已满玩滑翔跳转查看http://106.53.86.247/#/serverList");
                }
                //离开游戏
                if (deleteList.Any(t => t == playeInfo.Name))
                {
                    MailUtil.SendMessage("1422323984@qq.com", "1422323984@qq.com", "ffkmyvqapzyfbafb", "滑翔服玩家提示", $"玩家{playeInfo.Name}在地狱已满玩滑翔战败了，是个逃兵，快去群里嘲笑一番吧");
                }
            }
            _dataCache.SetOldPlayerInfoList(playerListInfo.Select(t=>t.Name).ToList());
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
