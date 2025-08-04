namespace Model.Dtos.Players;

public class PlayerWRDto
{
    /// <summary>
    ///     地图Id
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public string MapName { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    public string Difficulty { get; set; }

    /// <summary>
    ///     地图图片
    /// </summary>
    public string Img { get; set; }

    public List<PlayerStageDto> Stages { get; set; } = new();
}