using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using IServices.Base;
using Repository.BASE;
using Repository.UnitOfWorks;
using Serilog;
using Services.Base;

namespace Extensions.ServiceExtensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                Log.Error(msg);
                throw new Exception(msg);
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();     //注册服务

            // 获取 Service.dll 程序集服务，并注册
            builder.RegisterAssemblyTypes(Assembly.LoadFrom(servicesDllFile))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors(); 

            // 获取 Repository.dll 程序集服务，并注册
            builder.RegisterAssemblyTypes(Assembly.LoadFrom(repositoryDllFile))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();

            #endregion
        }
    }
}