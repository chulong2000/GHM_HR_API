using Autofac;
using GHM.HR.API.Domain.Resources;
using GHM.HR.API.Infrastructure.Data;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace GHM.HR.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Module
    {
        public string? ConnectionString { get; }
        public ApplicationModule(string? connectionString)
        {
            ConnectionString = connectionString;
   
        }
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            #region Repositories            
            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces()
              .WithParameter(new TypedParameter(typeof(string), ConnectionString));
            #endregion

            #region Services            
            // Scan các Service thông thường
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            #region Resources
            builder.RegisterType<ResourceService<GhmHRResource>>()
                .As<IResourceService<GhmHRResource>>()
                .InstancePerLifetimeScope();
            #endregion

            var appConnectionStrings = new Dictionary<string, string?>
            {
                { DbConnectionNames.Hr, ConnectionString }
            };
            builder.RegisterInstance(new SqlConnectionFactory(appConnectionStrings))
                .As<IDbConnectionFactory>()
                .SingleInstance();

            builder.RegisterType<DbSession>()
                .As<IDbSession>()
                .InstancePerLifetimeScope();
            #endregion
        }
    }
}
