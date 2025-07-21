using Microsoft.AspNetCore.Mvc;

namespace ClientWeb.Controllers
{
    /// <summary>
    /// Steam信息
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        [HttpGet("SendMessage")]
        public void SendMessage()
        {
        }
    }
}