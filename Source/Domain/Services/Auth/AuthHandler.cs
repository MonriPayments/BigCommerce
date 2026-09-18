using ILogger = WebStudio.Logging.Abstractions.ILogger;
using WebStudio.Entities.Interaction;
using WebStudio.Entities.Core;
using WebStudio.Common.Contracts;
using System.Threading.Tasks;
using System.Threading;
using System;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using System.Collections.Generic;
using BigCommerceApi.Domain.Services.RestManagementApi;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.RestManagementApi.Enums;

namespace BigCommerceApi.Domain.Services.Auth
{
    public class AuthHandler : BaseCommandHandler, IAsyncCommandHandler<AuthCommand, Response<AuthResult>>
    {
        private readonly ILogger _logger;
        private readonly IRestManagementApi _restManagementApi;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        private const string REDIRECT_LOAD_URI = "merchantonboarding";
        private const string DEFAULT_PAYMENT_METHOD_NAME = "Plaćanje karticama";

        public AuthHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger, IRestManagementApi restManagementApi, BigCommerceConfiguration bigCommerceConfiguration) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(logger, nameof(logger));
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));

            _logger = logger;
            _restManagementApi = restManagementApi;
            _bigCommerceConfiguration = bigCommerceConfiguration;
        }

        public async Task<Response<AuthResult>> ExecuteAsync(AuthCommand command, CancellationToken cancellationToken)
        {
            try
            {
                bool isNewStore = false;
                var response = new Response();
                var request = RestManagementApiHelper.GetAccessTokenRequest(command, _bigCommerceConfiguration);

                _logger.LogAPICommunication(Events.BigCAuthRequestReceived, new APIRequestLogState(nameof(AuthHandler), command.RequestId, request));

                var getAccessTokenResponse = await _restManagementApi.GetAccessTokenResponseAsync(request, _bigCommerceConfiguration.BigCAccessTokenPath!, null, cancellationToken);
            
                if (getAccessTokenResponse == null)
                {
                    return ResponseHelper.CreateErrorResponse<AuthResult>(ErrorMessages.FailedToGenerateAccessToken);
                }

                _logger.LogAPICommunication(Events.BigCAuthRequestReceived, new APIRequestLogState(nameof(AuthHandler), command.RequestId, getAccessTokenResponse));

                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.SubjectPath == command.Context);
                if (storeConfiguration == null)
                {
                    storeConfiguration = new StoreConfiguration();
                    isNewStore = true;
                }

                storeConfiguration.AccessToken = getAccessTokenResponse.AccessToken;
                storeConfiguration.ClientName = getAccessTokenResponse.Owner!.Username;
                storeConfiguration.ClientID = getAccessTokenResponse.AccountUuid;
                storeConfiguration.ClientSecret = getAccessTokenResponse.AccountUuid;
                storeConfiguration.StoreApiAccountName = getAccessTokenResponse.Owner!.Username;
                storeConfiguration.SubjectPath = getAccessTokenResponse.Context;
                storeConfiguration.ApiPath = $"https://api.bigcommerce.com/{getAccessTokenResponse.Context}/v3/";

                if (isNewStore)
                {
                    storeConfiguration.StoreStatus = StoreStatus.Test;
                    storeConfiguration.CheckoutType = CheckoutType.EmbeddedCheckout;
                    storeConfiguration.DefaultOrderStatus = OrderStatus.AwaitingFulfullment;
                    storeConfiguration.PaymentMethodName = DEFAULT_PAYMENT_METHOD_NAME;
                }

                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(storeConfiguration);
                await UnitOfWork.CommitAsync(cancellationToken);

                var scriptRequest = RestManagementApiHelper.CreateScriptRequest(
                    "Monri checkout", 
                    "Monri checkout helper script", 
                    $"{_bigCommerceConfiguration.AppUri}monri_custom_checkout.js",
                    Location.footer
                );

                _logger.LogAPICommunication(Events.BigCScriptCommunication, new APIRequestLogState(nameof(AuthHandler), command.RequestId, scriptRequest));

                var createFormScript = _restManagementApi.CreateScriptResponseAsync(
                    scriptRequest,
                    storeConfiguration.ApiPath,
                    getAccessTokenResponse.AccessToken!,
                    command.RequestId,
                    cancellationToken
                );

                _logger.LogAPICommunication(Events.BigCScriptCommunication, new APIRequestLogState(nameof(AuthHandler), command.RequestId, createFormScript.Result));

                var createResizeScript = _restManagementApi.CreateScriptResponseAsync(
                    RestManagementApiHelper.CreateScriptRequest("Monri iFrame resize", "Monri iframe resizer for WSPay form", $"{_bigCommerceConfiguration.AppUri}iframeResizer.min.js", Location.head),
                    storeConfiguration.ApiPath,
                    getAccessTokenResponse.AccessToken!,
                    command.RequestId,
                    cancellationToken
                );

                return new Response<AuthResult>(new AuthResult { Url = _bigCommerceConfiguration.AppUri + REDIRECT_LOAD_URI });

            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "AuthHandler" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<AuthResult>(ErrorMessages.GenericError);
            }
        } 
    }
}
