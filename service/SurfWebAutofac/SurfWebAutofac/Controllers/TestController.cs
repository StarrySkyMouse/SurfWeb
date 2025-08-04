using IServices;
using Microsoft.AspNetCore.Mvc;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     Steam信息
/// </summary>
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestServices _testServices;
    public TestController(ITestServices testServices)
    {
        _testServices = testServices;
    }
    /// <summary>
    ///     发送消息
    /// </summary>
    [HttpGet("SendMessage")]
    public async Task SendMessage()
    {
        await _testServices.Test();
    }
}