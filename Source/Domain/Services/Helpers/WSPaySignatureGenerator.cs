using WebStudio.Common.Cryptography;

namespace BigCommerceApi.Domain.Services.Helpers
{
	public static class WSPaySignatureGenerator
	{
		public static string GenerateForProcessPayment(string? shopId, string? secretKey, string? shoppingCartId, string? totalAmount)
		{
			var stringToHash = shopId + secretKey + shoppingCartId + secretKey + totalAmount + secretKey;
			return HashingUtility.ComputeHash(stringToHash, HashingProvider.SHA512).HexValue;
		}
	}
}
