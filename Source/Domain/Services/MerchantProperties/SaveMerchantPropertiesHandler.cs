using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Helpers;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.MerchantProperties
{
    public class SaveMerchantPropertiesHandler : BaseCommandHandler, IAsyncCommandHandler<SaveMerchantPropertiesCommand, Response<SaveMerchantPropertiesResult>>
    {
        public SaveMerchantPropertiesHandler(
            IQueryExecutor queryExecutor,
            UnitOfWork unitOfWork) : base(queryExecutor, unitOfWork)
        {
            
        }

        public async Task<Response<SaveMerchantPropertiesResult>> ExecuteAsync(SaveMerchantPropertiesCommand command, CancellationToken cancellationToken)
        {
            const string DEFAULT_PAYMENT_METHOD_NAME = "Plaćanje karticama";

            var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.SubjectPath == command.MerchantPath, cancellationToken);

            if (storeConfiguration == null)
                return ResponseHelper.CreateErrorResponse<SaveMerchantPropertiesResult>(ErrorMessages.StoreConfigurationNotFound);

            storeConfiguration.MerchantEmail = command.MerchantEmail;
            storeConfiguration.MerchantName = command.MerchantName;
            storeConfiguration.MerchantSiteUrl = command.MerchantSite;
            storeConfiguration.StoreStatus = command.IsInProduction ? StoreStatus.Production : StoreStatus.Test;
            storeConfiguration.CheckoutType = command.CheckoutType == "EmbeddedCheckout" ? CheckoutType.EmbeddedCheckout : CheckoutType.Redirect;
            storeConfiguration.PaymentMethodName = string.IsNullOrEmpty(command.PaymentMethodName) ? DEFAULT_PAYMENT_METHOD_NAME : command.PaymentMethodName;

            UnitOfWork.Begin();
            UnitOfWork.RegisterEntityToAddOrUpdate(storeConfiguration);

            await UnitOfWork.CommitAsync(cancellationToken);

            return new Response<SaveMerchantPropertiesResult>();
        }
    }
}
