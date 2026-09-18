using WebStudio.Common.Cryptography;

namespace BigCommerceApi.Domain.Services.WSPay
{
    public static class WSPaySignatureGenerator
    {
        public static string GenerateForAuthorizationAnnounce(string? shopId, string? secretKey, string? shoppingCartId)
        {
            var stringToHash = shopId + secretKey + shoppingCartId + secretKey + shopId + shoppingCartId;
            return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
        }

        public static string GenerateForProcessPayment(string? shopId, string? secretKey, string? shoppingCartId, string? totalAmount)
        {
            var stringToHash = shopId + secretKey + shoppingCartId + secretKey + totalAmount + secretKey;
            return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
        }

        public static string GenerateForAutoResponse(string? shopId, string? secretKey, string? shoppingCartId, string? approved, string? approvalCode)
        {
            var stringToHash = shopId + secretKey + shoppingCartId + secretKey + approved + secretKey + approvalCode + secretKey;
            return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
        }

        public static string GenerateForCheckPaymentStatus(string? shopId, string? secretKey, string? shoppingCartId)
        {
            var stringToHash = shopId + secretKey + shoppingCartId + secretKey + shopId + shoppingCartId;
            return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
        }

        public static string GenerateSignatureForTransactionUpdateRequest(string? shopId, string? wsPayOrderId, string? secretKey, string? STAN, string? approvalCode, string? amount)
        {
            var stringToHash = shopId + wsPayOrderId + secretKey + STAN + secretKey + approvalCode + secretKey + amount + secretKey + wsPayOrderId;

            return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
        }
    }
}
