using BigCommerceApi.Domain.Model.Transactions;
using BigCommerceApi.Domain.Services.Callback.OrderStatusUpdate;
using BigCommerceApi.Domain.Services.WSPay.Interaction;
namespace BigCommerceApi.Domain.Services.WSPay
{
    public static class WSPayTransactionManagementHelpers
    {
        public static T GenerateServiceRequest<T>(Transaction transaction, string secretKey) where T : IServiceRequest, new()
        {
            return new T
            {
                WsPayOrderId = transaction.WsPayOrderId,
                STAN = transaction.STAN,
                ApprovalCode = transaction.ApprovalCode,
                Amount = transaction.Amount.ToString(),
                ShopID = transaction.ShopID,
                Signature = WSPaySignatureGenerator.GenerateSignatureForTransactionUpdateRequest(
                    transaction.ShopID,
                    transaction.WsPayOrderId,
                    secretKey,
                    transaction.STAN,
                    transaction.ApprovalCode,
                    transaction.Amount.ToString()
                ),
                Marketplace = null
            };
        }

        public static OrderStatusUpdateResult GenerateServiceResponse<T>(T serviceResponse)
            where T : IServiceResponse
        {
            return new OrderStatusUpdateResult
            {
                ActionSuccess = serviceResponse?.Result?.ActionSuccess,
                WsPayOrderId = serviceResponse?.Result?.WsPayOrderId,
                Signature = serviceResponse?.Result?.Signature,
                STAN = serviceResponse?.Result?.STAN,
                ApprovalCode = serviceResponse?.Result?.ApprovalCode,
                ErrorMessage = serviceResponse?.Result?.ErrorMessage,
                ShopID = serviceResponse?.Result?.ShopID
            };
        }

    }
}
