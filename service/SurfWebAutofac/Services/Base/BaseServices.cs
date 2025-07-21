using IServices.Base;
using Model.Models.Base;
using Repository.BASE;

namespace Services.Base;

public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
{
}