using Core.Dapper;
using Core.Dapper.Entitys;
using Core.IServices;
using Core.Models;
using Core.Services;
using Quartz;

namespace Core.Utils.Jobs.SequenceJobs
{
    /// <summary>
    /// 记录同步
    /// </summary>
    public class PlayerCompleteSequenceJob : ISequenceJob
    {
        private readonly ISqlHelp _sqlHelp;
        private readonly IPlayerCompleteServices _playerCompleteServices;
        private readonly IPlayerServices _playerServices;
        private readonly IMapServices _mapServices;

        public PlayerCompleteSequenceJob(
            ISqlHelp sqlHelp,
            IPlayerCompleteServices playerCompleteServices,
            IPlayerServices playerServices,
            IMapServices mapServices)
        {
            _sqlHelp = sqlHelp;
            _playerCompleteServices = playerCompleteServices;
            _playerServices = playerServices;
            _mapServices = mapServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            if (_sqlHelp.IsDataSync())
            {
                //查询最后数据的时间
                var finallyDateTime = _playerCompleteServices.GetFinallyDateTime();
                //同步主线和奖励记录
                var syncData1 = await GetPlayertimesList(finallyDateTime.Item1);
                //同步阶段记录
                var syncData2 = await GetStagetimesList(finallyDateTime.Item2);
                var addList = new List<PlayerCompleteModel>();
                var updateList = new List<PlayerCompleteModel>();
                var authList = new List<int>();
                authList.AddRange(syncData1.Select(t => t.auth));
                authList.AddRange(syncData2.Select(t => t.auth));
                authList = authList.Distinct().ToList();
                var mapList = new List<string>();
                //通过Auth获取PlayerId
                var authDic = await _playerServices.GetPlayerInfoListByAuth(authList);
                //通过mapName获取MapId
                var mapNameList = new List<string>();
                mapNameList.AddRange(syncData1.Select(t => t.map));
                mapNameList.AddRange(syncData2.Select(t => t.map));
                mapNameList = mapNameList.Distinct().ToList();
                var mapDic = await _mapServices.GetMapIdListByName(mapNameList);
                if (syncData1.Any())
                {
                    //获取旧的数据
                    var oldList = await _playerCompleteServices.GetOldPlayertimesData(syncData1.Select(t => (
                        t.auth,
                        t.map,
                        t.track
                    )));
                    oldList.ForEach(t =>
                    {
                        var newData = syncData1.First(
                            a => t.Auth == a.auth &&
                            t.MapName == a.map &&
                            t.Type == (a.track == 0 ? RecordTypeEnum.Main : RecordTypeEnum.Bounty) &&
                            t.Stage == (a.track == 0 ? null : a.track)
                        );
                        t.Time = newData.time;
                        t.Date = DateTimeUtil.FromUnixTimestamp(newData.date);
                    });
                    updateList.AddRange(oldList);
                    addList.AddRange(syncData1
                        .Where(t => !oldList.Any(
                            a => a.Auth == t.auth &&
                            a.MapName == t.map &&
                            a.Type == (t.track == 0 ? RecordTypeEnum.Main : RecordTypeEnum.Bounty) &&
                            a.Stage == (t.track == 0 ? null : t.track)))
                        .Select(t =>
                        {
                            var playerInfo = authDic.ContainsKey(t.auth) ? authDic[t.auth] : default;
                            var mapId = mapDic.ContainsKey(t.map) ? mapDic[t.map] : null;
                            var result = new PlayerCompleteModel()
                            {
                                Id = null, // 该标识在插入时自动生成
                                Auth = t.auth,
                                PlayerId = playerInfo.Item1,
                                PlayerName = playerInfo.Item2,
                                MapId = mapId,
                                MapName = t.map,
                                Type = t.track == 0 ? RecordTypeEnum.Main : RecordTypeEnum.Bounty,
                                Stage = t.track == 0 ? null : t.track,
                                Time = t.time,
                                Date = DateTimeUtil.FromUnixTimestamp(t.date),
                            };
                            return result;
                        }));
                }
                if (syncData2.Any())
                {
                    //获取旧的数据
                    var oldList = await _playerCompleteServices.GetOldStagetimesData(syncData2.Select(t => (
                        t.auth,
                        t.map,
                        t.stage
                    )));
                    oldList.ForEach(t =>
                    {
                        var newData = syncData2.First(
                            a => t.Auth == a.auth &&
                            t.MapName == a.map &&
                            t.Type == RecordTypeEnum.Stage &&
                            t.Stage == a.stage
                        );
                        t.Time = newData.time;
                        t.Date = DateTimeUtil.FromUnixTimestamp(newData.date);
                    });
                    addList.AddRange(syncData2
                        .Where(t => !oldList.Any(
                            a => a.Auth == t.auth &&
                            a.MapName == t.map &&
                            a.Type == RecordTypeEnum.Stage &&
                            a.Stage == t.stage))
                        .Select(t =>
                        {
                            var playerInfo = authDic.ContainsKey(t.auth) ? authDic[t.auth] : default;
                            var mapId = mapDic.ContainsKey(t.map) ? mapDic[t.map] : null;
                            var result = new PlayerCompleteModel()
                            {
                                Id = null, // 该标识在插入时自动生成
                                Auth = t.auth,
                                PlayerId = playerInfo.Item1,
                                PlayerName = playerInfo.Item2,
                                MapId = mapId,
                                MapName = t.map,
                                Type = RecordTypeEnum.Stage,
                                Stage = t.stage,
                                Time = t.time,
                                Date = DateTimeUtil.FromUnixTimestamp(t.date),
                            };
                            return result;
                        })
                        );
                }
                if (addList.Any())
                {
                    _playerCompleteServices.Inserts(addList);
                }
                if (updateList.Any())
                {
                    _playerCompleteServices.Updates(updateList);
                }
            }
            //处理增量过来的数据还未完成关联的
            await _playerCompleteServices.DisposeDataAssociation();
            //隐藏不存在地图信息的数据
            await _playerCompleteServices.HideUnLikeData();
        }
        //增量查询主线和奖励记录
        private async Task<List<Playertimes>> GetPlayertimesList(DateTime? date)
        {
            var unixTimestamp = DateTimeUtil.ToUnixTimestamp(date);
            int pageIndex = 1;
            int pageSize = 1000;
            var result = new List<Playertimes>();
            while (true)
            {
                var playertimes = await _sqlHelp.QueryPageAsync<Playertimes>(
                    @"
                        select `style` ,track,`time` ,auth ,`map` ,`date`  
                        from playertimes 
                        where `style`=0 and `date` >=? 
                        order by `date` ", pageIndex, pageSize, new
                    {
                        unixTimestamp
                    });
                if (playertimes == null || !playertimes.Any())
                    break;
                result.AddRange(playertimes);
                pageIndex++;
            }
            //处理边界(时间完全一样的情况)
            if (result.Count > 0 && date != null)
            {
                var dbDate = await _playerCompleteServices.GetByDate((DateTime)date,
                    new List<RecordTypeEnum>() {
                        RecordTypeEnum.Main,RecordTypeEnum.Bounty
                    });
                if (dbDate.Any())
                {
                    var _dbDate = dbDate.Select(t => new Playertimes()
                    {
                        track = t.Type == RecordTypeEnum.Main ? 0 : t.Stage ?? -1,
                        time = t.Time,
                        auth = t.Auth,
                        map = t.MapName,
                        date = DateTimeUtil.ToUnixTimestamp(t.Date)
                    });
                    //去除已经存在的记录
                    result = result.Where(x => !_dbDate.Any(y =>
                        y.track == x.track &&
                        y.time == x.time &&
                        y.auth == x.auth &&
                        y.map == x.map &&
                        y.date == x.date)
                    ).ToList();
                }
            }
            return result;
        }
        //增量查询阶段记录
        private async Task<List<Stagetimes>> GetStagetimesList(DateTime? date)
        {
            var unixTimestamp = DateTimeUtil.ToUnixTimestamp(date);
            int pageIndex = 1;
            int pageSize = 1000;
            var result = new List<Stagetimes>();
            while (true)
            {
                var stagetimes = await _sqlHelp.QueryPageAsync<Stagetimes>(
                    @"
                        select stage ,`time` ,auth ,`map` ,`date` 
                        from stagetimes 
                        where `style`=0 and `date` >=? 
                        order by `date` ", pageIndex, pageSize, new
                    {
                        unixTimestamp
                    });
                if (stagetimes == null || !stagetimes.Any())
                    break;
                result.AddRange(stagetimes);
                pageIndex++;
            }
            //处理边界(时间完全一样的情况)
            if (result.Count > 0 && date != null)
            {
                var dbDate = await _playerCompleteServices.GetByDate((DateTime)date,
                    new List<RecordTypeEnum>() {
                        RecordTypeEnum.Stage
                    });
                if (dbDate.Any())
                {
                    var _dbDate = dbDate.Select(t => new Stagetimes()
                    {
                        stage = t.Stage ?? -1,
                        time = t.Time,
                        auth = t.Auth,
                        map = t.MapName,
                        date = DateTimeUtil.ToUnixTimestamp(t.Date)
                    });
                    //去除已经存在的记录
                    result = result.Where(x => !_dbDate.Any(y =>
                        y.stage == x.stage &&
                        y.time == x.time &&
                        y.auth == x.auth &&
                        y.map == x.map &&
                        y.date == x.date)
                    ).ToList();
                }
            }
            return result;
        }
    }
}