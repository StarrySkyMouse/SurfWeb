namespace Model.Dtos.Players;

public class PlayerStageDto
{
    /// <summary>
    ///     阶段
    /// </summary>
    public int? Stage { get; set; }

    /// <summary>
    ///     时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     和wr差距
    /// </summary>
    public float GapTime { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    public DateTime Date { get; set; }
}