using Common.Quartz.Base;
using IServices.Main;
using Model.ExteriorEntitys;
using Model.Models.Main;
using Quartz;
using Repository.Other;

namespace Configurations.QuartzSetup.SequenceJobs;

/// <summary>
///     用户数据同步前
/// </summary>
public class PlayerBeforeSequenceJob : ISequenceJob
{
    private readonly IPlayerServices _playerServices;
    private readonly ISqlHelp _sqlHelp;

    public PlayerBeforeSequenceJob(
        ISqlHelp sqlHelp,
        IPlayerServices playerServices)
    {
        _sqlHelp = sqlHelp;
        _playerServices = playerServices;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //没有修改时间字段，只能做全量同步

        //获取目标数据源和源数据源
        var targetList = await GetUsersFromTarget();
        //获取源数据源
        var sourceList = await GetUsersFromSource();
        var changeList = new List<PlayerModel>();
        var addList = new List<PlayerModel>();
        //List检索的时间复杂度是O(n)，而Dictionary检索的时间复杂度是O(1)
        var targetDict = targetList.ToDictionary(t => t.Auth, t => t);
        foreach (var src in sourceList)
            if (targetDict.TryGetValue(src.auth, out var tgt))
            {
                if (src.name != tgt.Name || src.points != tgt.Integral)
                {
                    tgt.Name = src.name;
                    tgt.Integral = src.points;
                    changeList.Add(tgt);
                }
            }
            else
            {
                addList.Add(new PlayerModel
                {
                    Auth = src.auth,
                    Name = src.name,
                    Integral = src.points
                });
            }

        if (changeList.Any()) await _playerServices.ChangeInfo(changeList);
        if (addList.Any()) _playerServices.Inserts(addList);
    }

    /// <summary>
    ///     获取目标数据源
    /// </summary>
    private async Task<List<PlayerModel>> GetUsersFromTarget()
    {
        var pageIndex = 1;
        var pageSize = 1000;
        var result = new List<PlayerModel>();
        while (true)
        {
            var players = await _playerServices.GetPlayerPageList(pageIndex, pageSize);
            if (players == null || !players.Any())
                break;
            result.AddRange(players);
            pageIndex++;
        }

        return result;
    }

    /// <summary>
    ///     获取源数据源
    /// </summary>
    private async Task<List<Users>> GetUsersFromSource()
    {
        var pageIndex = 1;
        var pageSize = 1000;
        var result = new List<Users>();
        while (true)
        {
            var users = await _sqlHelp.QueryPageAsync<Users>("select auth,name,points from users", pageIndex, pageSize);
            if (users == null || !users.Any())
                break;
            result.AddRange(users);
            pageIndex++;
        }

        return result;
    }
}