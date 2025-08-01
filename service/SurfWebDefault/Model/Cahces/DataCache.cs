using System.Collections.Immutable;

namespace Model.Cahces;

/// <summary>
///     缓存对象
/// </summary>
public class DataCache
{
    private readonly object _lock = new();

    /// <summary>
    ///     地图数据快照
    /// </summary>
    public MapCacheSnapshot MapSnapshot { get; private set; } = new(
        ImmutableList<MapMainCache>.Empty,
        ImmutableList<MapBountyOrStageCache>.Empty,
        ImmutableList<MapBountyOrStageCache>.Empty,
        ImmutableList<MapWrCache>.Empty
    );

    /// <summary>
    ///     服务器信息快照
    /// </summary>
    public ServiceInfoCache ServiceInfoSnapshot { get; private set; } = new();

    public void SetCacheSnapshot(MapCacheSnapshot snapshot)
    {
        lock (_lock)
        {
            MapSnapshot = snapshot;
        }
    }

    public void SetServiceInfoSnapshot(ServiceInfoCache snapshot)
    {
        lock (_lock)
        {
            ServiceInfoSnapshot = snapshot;
        }
    }

    /// <summary>
    ///     不可变集合，防止引用类型被外部修改
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
}