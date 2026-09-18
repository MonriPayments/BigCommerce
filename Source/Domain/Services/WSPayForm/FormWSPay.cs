using Newtonsoft.Json;
using RestSharp;
using System.Net;
using WebStudio.Common.Contracts;
using WebStudio.Logging.Abstractions;
using BigCommerceApi.Domain.Services.WSPayForm.Interaction;
using BigCommerceApi.Domain.Services.Logging;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace BigCommerceApi.Domain.Services.WSPayForm
{
	public sealed class FormWSPay : IFormWSPay
	{
		private readonly FormWSPayConfiguration _configuration;
		private readonly ILogger _logger;

		public FormWSPay(FormWSPayConfiguration configuration, ILogger logger)
		{
			Argument.IsNotNull(configuration, nameof(configuration));
			Argument.IsNotNull(logger, nameof(logger));

			_configuration = configuration;
			_logger = logger;
		}

		public async Task<CreateFormTransactionResponse> CreateFormTransactionAsync(CreateFormTransactionRequest request, string formUrl, CancellationToken cancellationToken)
		{
			return await GetResponse<CreateFormTransactionResponse.ServiceResponse, CreateFormTransactionResponse>(nameof(CreateFormTransactionAsync), _configuration.CreateTransactionUrlEndpoint, request, false, request.ShoppingCartID, formUrl, cancellationToken);
		}

		private async Task<TResponse> GetResponse<TResult, TResponse>(string apiName, string action, object request, bool addBasicAuth, string? requestId, string formUrl, CancellationToken cancellationToken = default) where TResult : class
		{
			_logger.LogAPICommunication(Events.WSPayAPIRequestSent, new APIRequestLogState(apiName, requestId, request, null, HttpMethod.Post, action));

			try
			{
				var restClient = new RestClient(formUrl);
				//if (addBasicAuth)
				//    restClient.Authenticator = new HttpBasicAuthenticator(_configuration.BasicAuthUsername!, _configuration.BasicAuthPassword!);

				var restRequest = new RestRequest(action, Method.Post)
				{
					Timeout = _configuration.DefaultTimeout
				};
				restRequest.AddJsonBody(request);
				var restResponse = await restClient.ExecuteAsync(restRequest, cancellationToken);

				if (restResponse.ErrorException != null)
					return (TResponse)Activator.CreateInstance(typeof(TResponse), null, restResponse.ErrorException)!;

				if (!restResponse.IsSuccessful)
					return (TResponse)Activator.CreateInstance(typeof(TResponse), null, new WebException($"{(int)restResponse.StatusCode} {restResponse.StatusDescription}", WebExceptionStatus.UnknownError))!;

				var result = JsonConvert.DeserializeObject<TResult>(restResponse.Content!);
				_logger.LogAPICommunication(Events.WSPayAPIResponseReceived, new APIResponseLogState(apiName, requestId, result, (int)restResponse.StatusCode, restResponse.ErrorException));

				return (TResponse)Activator.CreateInstance(typeof(TResponse), result, null)!;
			}
			catch (Exception exception)
			{
				_logger.LogAPICommunication(Events.WSPayAPIResponseError, new APIResponseLogState(apiName, requestId, exception));
				return (TResponse)Activator.CreateInstance(typeof(TResponse), null, exception)!;
			}
		}

	}
}
