using Microsoft.AspNetCore.Mvc;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;
namespace BigCommerceApi.Client.Web.Controllers
{

    [Route("trans")]
    public class TransactionStatusController : BaseController
    {

        public TransactionStatusController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger) : base(queryExecutor, commandExecutor, logger)
        {
        }

        //nova commanda 

        //novihandler koji ima novo

        //       switch (command.NewStatusId)
        //    {
        //        case OrderStatuses.Refunded:
        //            var serviceRefundResponse = await _wspayPaymentGateway.ServiceRefundAsync(
        ////todo check this for a call, maybe refactor
        //                WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceRefundRequest>(transaction, storeConfiguration),
        //                command.RequestId,
        //                cancellationToken
        //                );
        //            return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceRefundResponse>(serviceRefundResponse);
        //        case OrderStatuses.Completed:
        //            var serviceCompletionResponse = await _wspayPaymentGateway.ServiceCompletionAsync(
        //                WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceCompletionRequest>(transaction, storeConfiguration),
        //                command.RequestId,
        //                cancellationToken
        //                );
        //            return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceCompletionResponse>(serviceCompletionResponse);
        //        case OrderStatuses.Cancelled:
        //            var serviceVoidResponse = await _wspayPaymentGateway.ServiceVoidAsync(
        //                WSPayTransactionManagementHelpers.GenerateServiceRequest<ServiceVoidRequest>(transaction, storeConfiguration),
        //                command.RequestId,
        //                cancellationToken
        //                );
        //            return WSPayTransactionManagementHelpers.GenerateServiceResponse<ServiceVoidResponse>(serviceVoidResponse);
        //        default:
        //            return null;
        //    }
    }
}
