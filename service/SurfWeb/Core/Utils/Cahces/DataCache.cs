using Core.IServices;
using Core.Utils.Cahces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.GlobalParams
{
    public class DataCache
    {
        //由于DataCache是单例，所以需要注入的服务也必须是单例或作用域
        //IMapServices需要通过在方法被调用时获取，无法通过构造函数注入（生命周期不能冲突）
        private IServiceProvider _serviceProvider;
        private readonly object _lock = new();
        public DataCache(IServiceProvider serviceProvider)
        {
            _serviceProvider= serviceProvider;
        }
        /// <summary>
        /// 不可变集合，防止引用类型被外部修改
        /// </summary>
        public record CacheSnapshot(
            ImmutableList<MapMainCache> MapMainList,
            ImmutableList<MapBountyOrStageCache> MapBountyList,
            ImmutableList<MapBountyOrStageCache> MapStageList
        );

        private CacheSnapshot _snapshot = new(
            ImmutableList<MapMainCache>.Empty,
            ImmutableList<MapBountyOrStageCache>.Empty,
            ImmutableList<MapBountyOrStageCache>.Empty
        );
        public CacheSnapshot Snapshot => _snapshot;
        /// <summary>
        /// 更新数据
        /// </summary>
        public async Task UpdateCache()
        {
            //由于DataCache是单例注册，无法直接获取Scope的IMapServices，需要在方法中创建作用域
            //using 是C# 8.0+ 的简化写法它的作用是自动管理 scope 对象的生命周期
            //当 UpdateCache 方法执行到末尾scope.Dispose() 会被自动调用
            using var scope = _serviceProvider.CreateScope();
            var mapServices = scope.ServiceProvider.GetRequiredService<IMapServices>();
            var mapList = await mapServices.GetMapCacheList();
            var mainList = mapList
                .Select(m => new MapMainCache { Id = m.Id, Name = m.Name })
                .ToImmutableList();
            var bountyList = mapList
                .Where(t => t.BonusNumber != 0)
                .SelectMany(t =>
                    Enumerable.Range(1, t.BonusNumber)
                    .Select(b => new MapBountyOrStageCache
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Stage = b
                    }))
                .ToImmutableList();
            var stageList = mapList
                .Where(t => t.StageNumber != 0)
                .SelectMany(t =>
                    Enumerable.Range(1, t.StageNumber)
                    .Select(b => new MapBountyOrStageCache
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Stage = b
                    }))
                .ToImmutableList();
            var newSnapshot = new CacheSnapshot(mainList, bountyList, stageList);
            lock (_lock)
            {
                _snapshot = newSnapshot;
            }
        }
    }
}
