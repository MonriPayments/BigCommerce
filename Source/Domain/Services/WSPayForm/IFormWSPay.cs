using BigCommerceApi.Domain.Services.WSPayForm.Interaction;
using System.Threading;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Services.WSPayForm
{
	public interface IFormWSPay
	{
		public Task<CreateFormTransactionResponse> CreateFormTransactionAsync(CreateFormTransactionRequest request, string formUrl, CancellationToken cancellationToken);
	}
}
