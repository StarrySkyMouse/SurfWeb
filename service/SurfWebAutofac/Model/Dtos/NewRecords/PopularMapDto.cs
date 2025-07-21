namespace Model.Dtos.NewRecords;

public class PopularMapDto
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
    ///     完成人数
    /// </summary>
    public int SurcessNumber { get; set; }
}