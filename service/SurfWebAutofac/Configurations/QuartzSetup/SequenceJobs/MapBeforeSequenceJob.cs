using Common.Quartz.Base;
using IServices.Main;
using Model.ExteriorEntitys;
using Quartz;
using Repository.Other;

namespace Configurations.QuartzSetup.SequenceJobs;

/// <summary>
///     地图同步
/// </summary>
public class MapBeforeSequenceJob : ISequenceJob
{
    private readonly IMapServices _mapServices;
    private readonly ISqlHelp _sqlHelp;

    public MapBeforeSequenceJob(
        ISqlHelp sqlHelp,
        IMapServices mapServices
    )
    {
        _sqlHelp = sqlHelp;
        _mapServices = mapServices;
    }

    public async Task Execute(IJobExecutionContext context)
    {
            //查询地图奖励数和阶段数
            var mapInfoList = await _sqlHelp.QueryAsync<MapInfo>(@"
                        /*奖励-1*/
                        select map,COUNT(*)-1 as number ,1 type 
                        from mapzones
                        where `type` =0
                        group by map 
                        union all 
                        /*阶段+1*/
                        select map,COUNT(*)+1 as  number ,2 type 
                        from mapzones
                        where `type` =2
                        group by map ");
            //查询地图信息
            var mapModelList = await _mapServices.GetMapInfoByNameList(mapInfoList.Select(t => t.map));
            mapModelList.ForEach(map =>
            {
                //查询地图奖励数和阶段数
                var mapBonusNumber = mapInfoList.FirstOrDefault(t => t.map == map.Name && t.type == 1);
                map.BonusNumber = mapBonusNumber?.number ?? 0;
                var mapStageNumber = mapInfoList.FirstOrDefault(t => t.map == map.Name && t.type == 2);
                map.StageNumber = mapStageNumber?.number ?? 0;
            });
            _mapServices.Updates(mapModelList);
    }
}