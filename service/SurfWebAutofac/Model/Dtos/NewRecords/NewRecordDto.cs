using Model.Models;

namespace Model.Dtos.NewRecords;

public class NewRecordDto
{
    /// <summary>
    ///     玩家id
    /// </summary>
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    public string PlayerName { get; set; }

    /// <summary>
    ///     地图id
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public  string MapName { get; set; }

    /// <summary>
    ///     通关类型(主线，奖励，阶段)
    /// </summary>
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     说明难度或阶段
    /// </summary>
    public  string Notes { get; set; }

    /// <summary>
    ///     通关时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    //单独使用示例
    //[JsonConverter(typeof(DateTiemConverter))]
    public DateTime Date { get; set; }
}