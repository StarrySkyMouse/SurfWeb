using Common.Caches.Base;
using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Caches;
using Utils.Steam;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     Steam信息
/// </summary>
[ApiController]
[Route("[controller]")]
public class SteamController : ControllerBase
{
    private readonly ICache _cache;
    private readonly IMapServices _mapServices;
    private readonly IPlayerServices _playerServices;

    public SteamController(ICache cache, IMapServices mapServices, IPlayerServices playerServices)
    {
        _cache = cache;
        _mapServices = mapServices;
        _playerServices = playerServices;
    }

    /// <summary>
    ///     获取服务器信息
    /// </summary>
    [HttpGet("GetServerInfo")]
    public async Task<ServiceInfoCache?> GetServerInfo()
    {
        var result = await _cache.GetOrFunc("GetServerInfo", async () =>
        {
            var result = new ServiceInfoCache();
            var serverInfo = SteamUtil.GetServerInfo("124.223.198.48", 27070);
            var playerListInfo = SteamUtil.GetServerPlayerList("124.223.198.48", 27070);
            //名称查找地图
            var mapInfo = await _mapServices.GetMapInfoByName(serverInfo.Map);
            //名称查找玩家
            var playerList = await _playerServices.GetPlayersByNames(playerListInfo.Select(t => t.Name).ToList());
            result = new ServiceInfoCache
            {
                Map = serverInfo.Map,
                MapInfo = mapInfo == null
                    ? null
                    : new ServiceInfoMapInfoCache
                    {
                        Id = mapInfo.Id,
                        Difficulty = mapInfo.Difficulty,
                        Img = mapInfo.Img
                    },
                MaxPlayers = 9,
                PlayerInfos = playerListInfo.Select(t => new ServiceInfoPlayerInfoCache
                {
                    PlayerId = playerList.FirstOrDefault(p => p.Name == t.Name)?.Id ?? 0,
                    Name = t.Name,
                    Duration = t.Duration
                }).ToList()
            };
            return result;
        }, 30);
        return result;
    }
}