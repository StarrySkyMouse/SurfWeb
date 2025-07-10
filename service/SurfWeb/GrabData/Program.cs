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
    //var bspFileNames = GetPathMap();
    var bspFileNames = new List<string>() { "surf_anzchamps", "surf_ace", "surf_leesriize", "surf_progress_fix", "surf_tendies", "surf_zen", "surf_concretejungle", "surf_echo", "surf_garden", "surf_calycate", "surf_mostten_aerial_v1", "surf_atlantis", "surf_ethereal", "surf_rookie", "surf_sandtrap2", "surf_friday", "surf_summer_ksf", "surf_mesa", "surf_ardon_fix", "surf_acp", "surf_harmony_fix", "surf_fornax", "surf_8bit", "surf_nibiru", "surf_palais", "surf_not_so_hentai", "surf_cyberwave", "surf_arcade", "surf_egoing", "surf_rookie2", "surf_satellite_fix", "surf_ben10", "surf_aura", "surf_boreas", "surf_oasis", "surf_egypt2", "surf_nesquik", "surf_ameliorate", "surf_me", "surf_premium", "surf_botanica", "surf_sandtrap", "surf_n_turf", "surf_palm", "surf_nova", "surf_destruction", "surf_our", "surf_beginner2", "surf_fractal", "surf_summit", "surf_meme", "surf_kepler", "surf_extend", "surf_artois", "surf_kaaba", "surf_minty", "surf_hourglass", "surf_quirky", "surf_666", "surf_sunnyhappylove", "surf_frost", "surf_innokia", "surf_corruption", "surf_island", "surf_pantheon", "surf_lt_unicorn", "surf_forgotten", "surf_nyx", "surf_4head", "surf_beyond", "surf_training", "surf_nuclear", "surf_hades2", "surf_commune_beta11", "surf_flow_ksf", "surf_overgrowth", "surf_boogie_woogie", "surf_kitsune2", "surf_ebony", "surf_kitsune", "surf_delight", "surf_demise", "surf_agony", "surf_lovetunnel", "surf_beyond2", "surf_calycate2", "surf_acp_fix", "surf_nebula", "surf_garden_h", "surf_indecisive", "surf_pancake", "surf_orthodox", "surf_ivory", "surf_borderlands", "surf_calycate_ksf", "surf_tempest", "surf_boring", "surf_deathstar", "surf_deteriorate", "surf_whiteout", "surf_andromeda", "surf_interference", "surf_stonework4", "surf_angst", "surf_helloworld", "surf_leidenfrost", "surf_pyzire_fix", "surf_spectra", "surf_illusion", "surf_highlands", "surf_sanguine", "surf_trapped2", "surf_solace", "surf_through_hell", "surf_yellow", "surf_threnody", "surf_not_so_zen", "surf_mesa_mine", "surf_aqua_fix", "surf_greensway", "surf_bbb", "surf_epiphany", "surf_neutralize", "surf_hero", "surf_ravine", "surf_alexia_dark_a1", "surf_cannonball", "surf_everdark", "surf_bugs", "surf_derpis_ksf", "surf_utopia_njv", "surf_simpsons_source", "surf_beginner", "surf_christmas", "surf_greek_ksf", "surf_classics2", "surf_like_this", "surf_android_ksf", "surf_mesa_aether", "surf_guitar_hi", "surf_stonework", "surf_shodan", "surf_rubiks_cube", "surf_queen_of_the_ween", "surf_roman_v2", "surf_wicked", "surf_trapped", "surf_interstellar", "surf_stonework2", "surf_monotony_ksf", "surf_aquaflow", "surf_sirius", "surf_santorini2", "surf_dragon", "surf_conserve", "surf_resort", "surf_vast", "surf_treasurehunt", "surf_whynot2", "surf_whynot", "surf_elysium2", "surf_stonework3", "surf_runewords2_lod", "surf_zenith", "surf_christmas2", "surf_milkyway", "surf_antimatter2", "surf_nibiru2_dws", "surf_gradient", "surf_devil", "surf_drugs", "surf_beginner_hell", "surf_activation", "surf_shibboleth", "surf_rolly", "surf_aux", "surf_runewords2", "surf_pandora", "surf_antimatter_v2", "surf_elysium4", "surf_elysium" };
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