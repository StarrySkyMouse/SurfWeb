using ClosedXML.Excel;
using GrabData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Entitys;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Repositories;
using Utils.OSS;

IWebDriver _driver;
string _scanPath;
int _suspendTime;
AliyunOSSConfig _aliyunOSSConfig;
AliyunOSSUtil _aliyunOSSUtil;
SurfWebDbContext _dbContext;

Init();
await Scan();

#region function
//初始化
void Init()
{
    //当本地没有appsettings.Development.json时，使用开源配置
    var env = "Development";
    if (!File.Exists("appsettings.Development.json"))
    {
        env = "OpenSource";
    }
    var config = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())// Microsoft.Extensions.Configuration.FileExtensions 的扩展方法
       .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)//Microsoft.Extensions.Configuration.Json 的扩展方法
       .Build();

    var dbConfig = config.GetSection("DbConfig").Get<DbConfig>() ?? throw new Exception("无法获取DbConfig配置");//Microsoft.Extensions.Configuration.Binder的扩展方法
    _aliyunOSSConfig = config.GetSection("AliyunOSS").Get<AliyunOSSConfig>() ?? throw new Exception("无法获取AliyunOSS配置");
    _scanPath = config.GetSection("ScanPath").Get<string>() ?? throw new Exception("无法获取ScanPath配置");
    _suspendTime = config.GetSection("SuspendTime").Get<int>();
    _aliyunOSSUtil = new AliyunOSSUtil(_aliyunOSSConfig);

    var service = ChromeDriverService.CreateDefaultService();
    service.SuppressInitialDiagnosticInformation = true; // 不输出诊断信息
    service.HideCommandPromptWindow = true; // 隐藏命令行窗口
    var options = new ChromeOptions();
    //设置等待策略，默认onload事件完成，所有资源加载完成
    // 只等 DOMContentLoaded
    options.PageLoadStrategy = PageLoadStrategy.Eager;
    _driver = new ChromeDriver(service, options);

    _dbContext = new SurfWebDbContext(dbConfig);

    AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
    {
        _driver.Quit();
    };//控制台关闭时退出浏览器
}
//扫描
async Task Scan()
{
    Console.WriteLine("获取数据库中所有地图...");
    //获取数据库中所有地图
    var mapNameList = await _dbContext.Set<MapModel>().Select(t => t.Name).ToListAsync();
    Console.WriteLine("扫描路径下所有地图...");
    //扫描路径下所有地图
    var bspFileNames = GetPathMap();
    Console.WriteLine("获取新增地图...");
    //获取新增地图
    var addMapList = bspFileNames.Where(t => !mapNameList.Contains(t));
    if (addMapList.Count() == 0)
    {
        Console.WriteLine("没有新增地图");
    }
    else
    {
        Console.WriteLine($"扫描新增地图数量：{addMapList.Count()}");
        //读取Excel地图数据
        var mapInfos = ReadExcelMapInfo();
        foreach (var mapName in addMapList)
        {
            var imgInfo = await GetImgUrl(mapName);
            if (imgInfo.Item1)
            {
                var mapInfo = mapInfos.FirstOrDefault(t => t.Name?.ToUpper().Trim() == mapName.ToUpper().Trim());
                _dbContext.Set<MapModel>().Add(new MapModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = mapName.Trim(),
                    Difficulty = mapInfo?.Tier?.Trim() != null ? $"T{mapInfo?.Tier?.Trim()}" : "T0",
                    Img = imgInfo.Item2.Trim(),
                    CreateTime = DateTime.Now,
                    UpDateTime = DateTime.Now,
                    IsDelete = 0
                });
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"成功新增地图:{mapName.Trim()}");
            }
            else
            {
                Console.WriteLine("图片获取失败");
            }
            Thread.Sleep(_suspendTime);
        }
        Console.WriteLine("以完成");
    }
    Console.WriteLine("按任意键结束");
    Console.ReadKey();
}
//扫描路径下所有地
List<string> GetPathMap()
{
    // 获取所有 .bsp 文件的完整路径
    var bspFiles = Directory.GetFiles(_scanPath, "*.bsp");
    // 获取新增地图，只筛选出以 surf_ 开头的地图文件名称
    var bspFileNames = bspFiles
        .Select(t => Path.GetFileName(t).Replace(".bsp", ""))
        .Where(name => name.StartsWith("surf_", StringComparison.OrdinalIgnoreCase))
        .ToList();
    return bspFileNames;
}
//获取地图url（网页搜索图片下载上传到OSS）
async Task<(bool, string)> GetImgUrl(string mpaName)
{
    // 可选：指定ChromeDriver的路径，如果已加到环境变量则无需设置
    // var chromeDriverService = ChromeDriverService.CreateDefaultService("path/to/chromedriver");
    var mapUrl = "";
    var flag = true;
    try
    {
        // 打开网页
        _driver.Navigate().GoToUrl("https://gamebanana.com/search?_sOrder=best_match&_sSearchString=" + mpaName);
        var searchResult = false;
        // 等待 class 为 PreviewImage 的元素出现，最多等待30秒
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        wait.Until(t =>
        {
            var find1 = t.FindElements(By.ClassName("PreviewImage"));
            var find2 = t.FindElements(By.ClassName("GreyColor"));
            if (find1.Count > 0 || find2.Count > 0)
            {
                searchResult = find1.Count > 0;
                return find1.Count > 0 ? find1[0] : find2[0];
            }
            else
            {
                return null;
            }
        });
        // 查找内容
        var searchBox = _driver.FindElements(By.ClassName("PreviewImage"));
        if (searchResult && searchBox.Count > 0)
        {
            var imgUrl = searchBox[0].GetAttribute("src");
            using var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync(imgUrl);
            //上传到阿里云OSS
            _aliyunOSSUtil.Upload($"surfImg/{mpaName}.jpg", bytes);
            mapUrl = $"https://{_aliyunOSSConfig.BucketName}.{_aliyunOSSConfig.Endpoint.Replace("https://", "")}/surfImg/{mpaName}.jpg";
        }
        else
        {
            mapUrl = $"https://{_aliyunOSSConfig.BucketName}.{_aliyunOSSConfig.Endpoint.Replace("https://", "")}/surfImg/default.jpg";
        }
    }
    catch (Exception ex)
    {
        flag = false;
        Console.WriteLine(ex.ToString());
    }
    return (flag, mapUrl);
}
//获取Excel数据
List<ExcelMapInfo> ReadExcelMapInfo()
{
    using (var workbook = new XLWorkbook("0_Surf maps on KSF - CSS.xlsx"))
    {
        var result = workbook.Worksheet(1).RowsUsed()
             .Select(row => new ExcelMapInfo()
             {
                 Name = row.Cell(1).GetString(),
                 Tier = row.Cell(2).GetString(),
                 Type = row.Cell(3).GetString(),
             }).ToList();
        if (result.Count > 0)
        {
            result.RemoveAt(0);
        }
        return result.Where(t => !string.IsNullOrWhiteSpace(t.Name)).ToList();
    }
}
#endregion