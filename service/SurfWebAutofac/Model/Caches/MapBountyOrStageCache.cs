namespace Model.Caches;

public class MapBountyOrStageCache
{
    /// <summary>
    ///     地图ID
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    public required string Difficulty { get; set; }

    /// <summary>
    ///     图片
    /// </summary>
    public string Img { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    public int Stage { get; set; }
}