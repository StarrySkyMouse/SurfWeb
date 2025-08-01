using System.ComponentModel.DataAnnotations;
using Model.Entitys.Base;

namespace Model.Entitys;

/// <summary>
///     玩家通关
/// </summary>
public class PlayerCompleteModel : BaseEntity
{
    /// <summary>
    ///     腐竹玩家id
    /// </summary>
    public int Auth { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    [MaxLength(64)]
    public string? PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    [MaxLength(128)]
    public string? PlayerName { get; set; }

    /// <summary>
    ///     地图id
    /// </summary>
    [MaxLength(64)]
    public string? MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    [MaxLength(128)]
    public string? MapName { get; set; }

    /// <summary>
    ///     通关类型(主线，奖励，阶段)
    /// </summary>
    [MaxLength(1)]
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    [MaxLength(2)]
    public int? Stage { get; set; }

    /// <summary>
    ///     通关时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     是否隐藏
    /// </summary>
    public bool Hide { get; set; }
}