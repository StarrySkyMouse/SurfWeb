﻿namespace Model.Dtos.Players;

public class PlayerFailDto
{
    /// <summary>
    ///     地图id
    /// </summary>
    public string MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public string MapName { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    public string Difficulty { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    public List<int> Stages { get; set; } = new();
}