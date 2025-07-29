namespace Model.ExteriorEntitys;

/// <summary>
///     阶段纪录
/// </summary>
public class Stagetimes
{
    ///// <summary>
    ///// 模式正常或tas
    ///// </summary>
    //public int style { get; set; }
    /// <summary>
    ///     阶段
    /// </summary>
    public int stage { get; set; }

    /// <summary>
    ///     通关时间
    /// </summary>
    public float time { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    public int auth { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public string map { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    public long date { get; set; }
}