using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.RestManagementApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.Merchant
{
    public class SaveMerchantAccountHandler : BaseCommandHandler, IAsyncCommandHandler<SaveMerchantAccountCommand, Response<SaveMerchantAccountResult>>
    {
        private readonly IRestManagementApi _restManagementApi;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        private readonly ILogger _logger;

        public SaveMerchantAccountHandler(
            IQueryExecutor queryExecutor, 
            UnitOfWork unitOfWork, 
            IRestManagementApi restManagementApi,
            BigCommerceConfiguration bigCommerceConfiguration, 
            ILogger logger) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(restManagementApi, nameof(restManagementApi));
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));
            Argument.IsNotNull(logger, nameof(logger));

            _restManagementApi = restManagementApi;
            _bigCommerceConfiguration = bigCommerceConfiguration;
            _logger = logger;
        }

        public async Task<Response<SaveMerchantAccountResult>> ExecuteAsync(SaveMerchantAccountCommand command, CancellationToken cancellationToken)
        {
            string requestId = new Guid().ToString();
            try
            {
                var response = new Response();

                var shops = new BigCommerceApi.Domain.Model.Shops.Shops
                {
                    ShopID = command.ShopId,
                    SecretKey = command.SecretKey,
                    Language = "HR"
                };

                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(shops);
                await UnitOfWork.CommitAsync(cancellationToken);

                var storeConfiguration = new StoreConfiguration
                {
                    AccessToken = command.AccessToken,
                    ClientName = command.ClientName,
                    ClientID = command.ClientId,
                    ClientSecret = command.ClientSecret,
                    StoreApiAccountName = command.Name,
                    ApiPath = command.ApiPath,
                    MerchantSiteUrl = command.MerchantSiteUrl
                };

                UnitOfWork.Begin();
                UnitOfWork.RegisterEntityToAddOrUpdate(storeConfiguration);
                await UnitOfWork.CommitAsync(cancellationToken);

                var createScriptResponse = await _restManagementApi.CreateWebhookResponseAsync(
                    RestManagementApiHelper.CreateWebhookRequest(_bigCommerceConfiguration.AppUri!), 
                    command.ApiPath!, 
                    command.AccessToken!, 
                    requestId, 
                    cancellationToken
                );

                if (createScriptResponse == null)
                {
                    return ResponseHelper.CreateErrorResponse<SaveMerchantAccountResult>(ErrorMessages.MerchantAccountCreationFailed);
                }

                _logger.LogAPICommunication(
                    Events.BigCCreateWebhookRequestReceived, 
                    new APIRequestLogState(
                        JsonConvert.SerializeObject(createScriptResponse), 
                        requestId, 
                        null, 
                        HttpMethod.Post, 
                        "SaveMerchantAccountHandler"
                        )
                    );

                return ResponseHelper.CreateSuccessResponse<SaveMerchantAccountResult>(); ;
            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                {
                    { "Handler", "SaveMerchantAccountHandlers" },
                };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<SaveMerchantAccountResult>(ErrorMessages.GenericError);
            }
        }
    }
}
