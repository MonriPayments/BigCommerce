using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Model.Users;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.RestManagementApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Common.Collections;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Domain.Services.CustomerTokens
{
    public class CustomerTokensHandler : BaseCommandHandler, IAsyncCommandHandler<CustomerTokensCommand, Response<CustomerTokensResult>>
    {
        private readonly ILogger _logger;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;
        public CustomerTokensHandler(IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger, BigCommerceConfiguration bigCommerceConfiguration) : base(queryExecutor, unitOfWork)
        {
            Argument.IsNotNull(logger, nameof(logger));
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));

            _logger = logger;
            _bigCommerceConfiguration = bigCommerceConfiguration;
        }

        public async Task<Response<CustomerTokensResult>> ExecuteAsync(CustomerTokensCommand command, CancellationToken cancellationToken)
        {
            // Create the new credit card option
            var newCreditCardOption = new CustomerTokens
            {
                ImageSource = "",
                Token = "",
                TokenNumber = "",
                PaymentType = "",
                MaskedPan = "Dodaj novu karticu"
            };

            var customerTokenList = new List<CustomerTokens>
            {
                newCreditCardOption
            };

            if (!string.IsNullOrEmpty(command.Email))
            {
                var storeConfiguration = await QueryExecutor.GetOneAsync<StoreConfiguration>(_ => _.SubjectPath == command.StoreHash, cancellationToken);

                if (storeConfiguration != null)
                {
                    var bigCUserList = await QueryExecutor.GetOneAsync<User>(_ => _.Email == command.Email && _.IdStoreConfiguration == storeConfiguration.Id);

                    if (bigCUserList == null)
                        return new Response<CustomerTokensResult>(new CustomerTokensResult { });

                    var userTokens = await QueryExecutor.GetAllAsync<UserTokens>(_ => _.User == bigCUserList && _.CreditCardExpirationDate >= DateTime.Now);

                    if (userTokens == null)
                        return new Response<CustomerTokensResult>(new CustomerTokensResult { });

                    customerTokenList.AddMany(CustomerTokensHelper.GetCustomerTokensList(userTokens.ToList(), _bigCommerceConfiguration.AppUri!));                }
                
            }                      

            return new Response<CustomerTokensResult>(new CustomerTokensResult { CustomerTokens = customerTokenList });
        }
    }
}
