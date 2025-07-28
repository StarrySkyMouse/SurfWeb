using IServices.Base;
using Model.Models.Base;

namespace IServices.Main.Base;

/// <summary>
///     在弄一个IMainBaseServices的原因是注册的AOP拦截器要区分日志和业务服务，日志在注册拦截器会陷入到执行的循环
/// </summary>
public interface IMainBaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity, new()
{
}