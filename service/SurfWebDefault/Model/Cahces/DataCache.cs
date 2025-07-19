using System.Collections.Immutable;

namespace Model.Cahces
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    public class DataCache
    {
        private readonly object _lock = new();
        /// <summary>
        /// 不可变集合，防止引用类型被外部修改
        /// </summary>
        public record MapCacheSnapshot(
            //主线
            ImmutableList<MapMainCache> MapMainList,
            //奖励
            ImmutableList<MapBountyOrStageCache> MapBountyList,
            //阶段
            ImmutableList<MapBountyOrStageCache> MapStageList,
            //地图wr列表
            ImmutableList<MapWrCache> MapWrCaches
        );
        private MapCacheSnapshot _mapSnapshot = new(
            ImmutableList<MapMainCache>.Empty,
            ImmutableList<MapBountyOrStageCache>.Empty,
            ImmutableList<MapBountyOrStageCache>.Empty,
            ImmutableList<MapWrCache>.Empty
        );
        private ServiceInfoCache _serviceInfoSnapshot = new();
        //上个缓存的玩家列表
        private List<string> _oldPlayerInfoList = new List<string>();
        /// <summary>
        /// 地图数据快照
        /// </summary>
        public MapCacheSnapshot MapSnapshot => _mapSnapshot;
        /// <summary>
        /// 服务器信息快照
        /// </summary>
        public ServiceInfoCache ServiceInfoSnapshot => _serviceInfoSnapshot;
        public List<string> OldPlayerInfoList => _oldPlayerInfoList;
        public void SetCacheSnapshot(MapCacheSnapshot snapshot)
        {
            lock (_lock)
            {
                _mapSnapshot = snapshot;
            }
        }
        public void SetServiceInfoSnapshot(ServiceInfoCache snapshot)
        {
            lock (_lock)
            {
                _serviceInfoSnapshot = snapshot;
            }
        }
        public void SetOldPlayerInfoList(List<string> oldPlayerInfoList)
        {
            lock (_lock)
            {
                _oldPlayerInfoList = oldPlayerInfoList;
            }
        }
    }
}
