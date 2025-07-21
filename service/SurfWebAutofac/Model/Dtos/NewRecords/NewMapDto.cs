namespace Model.Dtos.NewRecords;

public class NewMapDto
{
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

    /// <summary>
    ///     创建日期
    /// </summary>
    public DateTime CreateTime { get; set; }
}