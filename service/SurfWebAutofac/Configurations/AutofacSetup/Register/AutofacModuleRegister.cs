using Autofac;
using Services.Main.ExtensionsRepository;
using Module = Autofac.Module;

namespace Configurations.AutofacSetup.Register;

public class AutofacModuleRegister : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType(typeof(PlayerCompleteRepository))
            .InstancePerLifetimeScope();
    }
}