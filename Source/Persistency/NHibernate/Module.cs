using System.Reflection;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using WebStudio.Common.Contracts;
using WebStudio.DependencyInjection.Autofac;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using BigCommerceApi.Domain.Model.Notifications;
using BigCommerceApi.Persistency.NHibernate.Interceptors;

namespace BigCommerceApi.Persistency.NHibernate
{
    public class Module : Autofac.Module
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public Module(DatabaseConfiguration databaseConfiguration)
        {
            Argument.IsNotNull(databaseConfiguration, nameof(databaseConfiguration));

            _databaseConfiguration = databaseConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterType<WebStudio.Persistency.NHibernateCore.UnitOfWork>().As<UnitOfWork>().InstancePerLifetimeScope();

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(_databaseConfiguration.ConnectionString)
                    .DefaultSchema(_databaseConfiguration.DefaultSchema)
                    .ShowSql()
                    .FormatSql())
                .ExposeConfiguration(cfg => cfg.SetProperty(global::NHibernate.Cfg.Environment.CommandTimeout, _databaseConfiguration.CommandTimeout.ToString()))
                .Mappings(mappings =>
                {
                    mappings.FluentMappings.AddFromAssemblyOf<Module>();
                    mappings.HbmMappings.AddFromAssemblyOf<Module>();
                    mappings.FluentMappings
                        .Conventions.AddFromAssemblyOf<Module>()
                        .Conventions.AddFromAssemblyOf<WebStudio.Persistency.NHibernateCore.Conventions.EntityIdConvention>()
                        .Conventions.Add(AutoImport.Never(), DefaultLazy.Never());
                })
                .BuildSessionFactory();

            builder.RegisterInstance(sessionFactory).As<global::NHibernate.ISessionFactory>().SingleInstance();
            builder.Register(c =>
            {
#if (DEBUG)
                var interceptor = new SqlDebugOutputInterceptor();
                return sessionFactory.WithOptions()
                    .Interceptor(interceptor)
                    .OpenSession();
#else
    return sessionFactory.OpenSession();
#endif
            }).As<global::NHibernate.ISession>().InstancePerLifetimeScope();

			builder.RegisterType<WebStudio.Persistency.NHibernateCore.UnitOfWork>().As<UnitOfWork>().InstancePerLifetimeScope();

			StandardInteractionHandlerRegistration.RegisterStandardInteractionHandlersForEntities(builder, Assembly.GetAssembly(typeof(WebStudio.Persistency.NHibernateCore.UnitOfWork))!, Assembly.GetAssembly(typeof(Notification))!);

			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncCommandHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncQueryHandler<,>));

            base.Load(builder);
        }
    }
}
