using Autofac;
using ChatApp.Data;

namespace ChatApp.Service.App_Start
{
    public class AutofacContext : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(TulparDbContext)).AsSelf().InstancePerLifetimeScope();
        }

    }
}
