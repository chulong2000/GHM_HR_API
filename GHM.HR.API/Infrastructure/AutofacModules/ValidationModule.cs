using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace GHM.HR.API.Infrastructure.AutofacModules
{
    public class ValidationModule : Module
    {
        public ValidationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.Name.EndsWith("Validator"))
                .AsImplementedInterfaces();
        }
    }
}
