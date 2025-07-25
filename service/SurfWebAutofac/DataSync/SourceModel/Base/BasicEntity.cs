using System.ComponentModel.DataAnnotations;

namespace DataSync.SourceModel.Base
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [MaxLength(64)]
        public required string Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpDateTime { get; set; }
        /// <summary>
        /// 删除标志0-未删除，1-删除
        /// </summary>
        [MaxLength(1)]
        public int IsDelete { get; set; }
    }
}
