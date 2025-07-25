using Microsoft.AspNetCore.Mvc;

namespace ClientWeb.Controllers
{
    /// <summary>
    /// Steam信息
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {

        //private readonly DataCache _dataCache;

        //public SteamController(DataCache dataCache)
        //{
        //    _dataCache = dataCache;
        //}
        ///// <summary>
        ///// 获取服务器信息
        ///// </summary>
        //[HttpGet("GetServerInfo")]
        //public ServiceInfoCache GetServerInfo()
        //{
        //    return _dataCache.ServiceInfoSnapshot;
        //}
    }
}