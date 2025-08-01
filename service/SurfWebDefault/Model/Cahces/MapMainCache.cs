﻿namespace Model.Cahces;

public class MapMainCache
{
    /// <summary>
    ///     地图ID
    /// </summary>
    public required string Id { get; set; }

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
}