using IServices.Base;
using Model.Models.Base;

namespace Services.Base;

public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
{
}