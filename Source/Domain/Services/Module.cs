using Autofac;
using System.Reflection;
using WebStudio.Entities.Interaction;
using BigCommerceApi.Domain.Services.PaymentGateways;
using BigCommerceApi.Domain.Services.PaymentGateways.WSPay;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Services.WSPayForm;
using Microsoft.Extensions.Caching.Memory;

namespace BigCommerceApi.Domain.Services
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncCommandHandler<,>));
			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<,>));

			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncQueryHandler<,>));
			builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IQueryHandler<,>));

			builder.RegisterAssemblyTypes(assembly).PublicOnly().AsClosedTypesOf(typeof(FluentValidation.IValidator<>));

			builder.Register(c =>
			{
				var memoryCache = new MemoryCache(new MemoryCacheOptions());
				return memoryCache;
			}).As<IMemoryCache>().SingleInstance();

			builder.RegisterType<CommandExecutor>().As<ICommandExecutor>();
			builder.RegisterType<QueryExecutor>().As<IQueryExecutor>();

			builder.RegisterType<InteractionObjectResolver>().As<IInteractionHandlerResolver, IQueryResultCacheResolver>();
			builder.RegisterType<RestManagementApi.RestManagementApi>().As<IRestManagementApi>();
			builder.RegisterType<FormWSPay>().As<IFormWSPay>();

			builder.RegisterType<WSPayGateway>().As<IPaymentGateway>().SingleInstance();
			builder.RegisterType<PaymentGatewayResolver>().As<IPaymentGatewayResolver>().SingleInstance();

			base.Load(builder);
        }
    }
}
