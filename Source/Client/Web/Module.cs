using Autofac;
using WebStudio.Common.Contracts;
using WebStudio.Logging.MSSQLServer;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web
{
	public sealed class Module : Autofac.Module
	{
		private readonly string _applicationName;

		public Module(string applicationName)
		{
			Argument.IsNotNullOrWhiteSpace(applicationName, nameof(applicationName));

			_applicationName = applicationName;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MSSQLServerLogger>().As<ILogger>().SingleInstance();

			//var getShopPaymentConfigurationQueryResultCache = new AutoInvalidatingInMemoryResultCache<GetShopPaymentConfigurationQuery, Response<GetShopPaymentConfigurationResult?>>(TimeSpan.FromHours(1));
			//builder.RegisterInstance(getShopPaymentConfigurationQueryResultCache).As<IQueryResultCache<GetShopPaymentConfigurationQuery, Response<GetShopPaymentConfigurationResult?>>>().SingleInstance();

			base.Load(builder);
		}
	}
}
