namespace Model.Dtos.Maps;

public class MapListDto
{
    /// <summary>
    ///     地图ID
    /// </summary>
    public  long Id { get; set; }

    /// <summary>
    ///     名称
    /// </summary>
    public  string Name { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    public  string Difficulty { get; set; }

    /// <summary>
    ///     地图图片
    /// </summary>
    public  string Img { get; set; }
}