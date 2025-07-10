using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using Utils.Steam;
using static Utils.Steam.SteamServerQuery;

namespace ClientWeb.Controllers
{
    /// <summary>
    /// Steam信息
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly IMapServices _mapServices;
        public SteamController(IMapServices mapServices)
        {
            _mapServices = mapServices;
        }
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        [HttpGet("GetServerInfo")]
        public async Task<object> GetServerInfo()
        {
            var result = SteamUtil.GetServerInfo("124.223.198.48", 27070);
            var mapInfoList = await _mapServices.GetMapInfoByNameList(new List<string>() { result.Map });
            return new
            {
                result.Name,
                result.Map,
                MapInfo = mapInfoList.Any() ? mapInfoList[0] : null,
                result.Players,
                MaxPlayers = 9
            };
        }
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        [HttpGet("GetServerPlayerList")]
        public List<PlayerInfo> GetServerPlayerList()
        {
            return SteamUtil.GetServerPlayerList("124.223.198.48", 27070);
        }
    }
}