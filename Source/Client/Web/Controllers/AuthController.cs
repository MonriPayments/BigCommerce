using BigCommerceApi.Client.Web.Extensions;
using BigCommerceApi.Client.Web.Helpers;
using BigCommerceApi.Client.Web.Interaction.Requests;
using BigCommerceApi.Domain.Projections.Shops;
using BigCommerceApi.Domain.Projections.Transactions;
using BigCommerceApi.Domain.Services.Cache;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.RestManagementApi;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebStudio.Common.Contracts;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Client.Web.Controllers
{
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly IWebAppCache _memoryCache;
        private readonly BigCommerceConfiguration _bigCommerceConfiguration;

        public AuthController(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor, ILogger logger, IWebAppCache memoryCache, BigCommerceConfiguration bigCommerceConfiguration) : base(queryExecutor, commandExecutor, logger)
        {
            Argument.IsNotNull(bigCommerceConfiguration, nameof(bigCommerceConfiguration));
            Argument.IsNotNull(memoryCache, nameof(memoryCache));

            _bigCommerceConfiguration = bigCommerceConfiguration;
            _memoryCache = memoryCache;
        }  

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AuthRequest authRequest, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();

            if (authRequest == null)
                return BadRequest();

            Logger.LogAPICommunication(Events.BigCAuthRequestReceived, new APIRequestLogState(nameof(Index), requestId, authRequest, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            var domainRequest = authRequest.CreateDomainRequest();
            domainRequest.RequestId = requestId;

            var domainResponse = await CommandExecutor.ExecuteAsync(domainRequest, cancellationToken);

            if (domainResponse.HasErrors)
            {
                Logger.LogDomainResponseError(new DomainResponseErrorLogState(nameof(Index), requestId, domainResponse));
                Logger.LogAPICommunication(Events.BigCAuthResponseSent, new APIResponseLogState(nameof(Index), requestId, domainResponse, 400));
                return BadRequest(domainResponse);
            }

            Logger.LogAPICommunication(Events.BigCAuthResponseSent, new APIResponseLogState(nameof(Index), requestId, domainResponse, 200));

            //StringBuilder tableRows = new StringBuilder();
            //DateTime effectiveStartDate = DateTime.Today.AddDays(-10);
            //DateTime effectiveEndDate =  DateTime.Today;

            string htmlMarkup = $@"
                <!DOCTYPE html>
                <html lang='en'>
                    {HtmlHelper.GetHtmlHeader("Order Details")}
                    <body style=""background: linear-gradient(90deg, #BE93D3 0%, #3196D3 100%);"">
                        <header class=""text-center py-4"">
                            <div class=""container"">
                                <a href=""#"" class=""logo"">
                                    <img src=""assets/images/monri-logo-white.svg"" alt=""Monri Payments logo"">
                                </a>
                            </div>
                        </header>
                        <main class=""d-flex flex-column align-items-center py-5"">
                            <div class=""text-center"">
                                <h1 class=""text-gradient"">Thank You for Installing Our App!</h1>
                                <p class=""text-gradient"">We're thrilled to have you onboard. Explore our guides to get started and make the most of our app.</p>
                            </div>

                            <div class=""mt-5"" style=""display: flex; flex-direction: column"">
                                <a href=""{_bigCommerceConfiguration.AppUri!}WSPayByMonri-BigCommerceUserGuide.pdf"" target=""_blank"" class=""btn btn-outline-primary mb-2 highlighted"">Getting Started Guide</a><br>
                                <a href=""https://monri.com"" target=""_blank"" class=""btn btn-outline-primary mb-2 highlighted"">Visit our page</a><br>
                            </div>
                        </main>
                    </body>
                </html>";

            return Content(htmlMarkup, "text/html");
        }

        [HttpGet]
        [Route("remove-user")]
        public IActionResult RemoveUser([FromQuery] LoadRequest loadRequest)
        {
            return Ok();
        }

        [HttpGet]
        [Route("load")]
        public async Task<IActionResult> Load([FromQuery] LoadRequest loadRequest, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();

            if (loadRequest == null)
                return BadRequest();

            Logger.LogAPICommunication(Events.BigCLoadRequestReceived, new APIRequestLogState(nameof(Load), requestId, loadRequest, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            var jwtTokenDecoded = JwtDecoder.DecodeJwt<LoadRequestJwt>(loadRequest.SignedPayloadJwt!, _bigCommerceConfiguration.AppClientID!);

            Logger.LogAPICommunication(Events.BigCLoadRequestReceived, new APIRequestLogState(nameof(Load), requestId, jwtTokenDecoded, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            StringBuilder tableRows = new StringBuilder();
            DateTime effectiveStartDate = DateTime.Today.AddDays(-10);
            DateTime effectiveEndDate = DateTime.Today;

            var merchantPropertiesQuery = new GetMerchantPropertiesQuery { SubjectPath = jwtTokenDecoded!.Sub };

            var merchantPropertiesList = await QueryExecutor.ExecuteAsync(merchantPropertiesQuery, cancellationToken);

            Logger.LogAPICommunication(Events.BigCGetMerchantProp, new APIRequestLogState(nameof(Index), requestId, merchantPropertiesList, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            // Fetch transactions based on provided dates or default to current day
            var query = new GetTransactionsQuery
            {
                CreatedFrom = effectiveStartDate,
                CreatedUntil = effectiveEndDate,
                PageSize = 10,
                PageNumber = 1,
                ShopID = string.Join("|", merchantPropertiesList.Select(_ => _.ShopIdOption))
            };

            var transactions = await QueryExecutor.ExecuteAsync(query, cancellationToken);

            var referer = string.Empty;

            if (HttpContext.GetRequestHeaders().TryGetValue("Referer", out referer)) {

            }

            if (transactions != null && transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    tableRows.Append($@"
                    <tr>
                        <td class=""text-start"">{transaction.TransactionDateTime.ToString("dd.MM.yyyy. HH:mm")}</td>
                        <td class=""text-start"">{transaction.ShopID}</td>
                        <td class=""text-start"">{transaction.CustomerName}</td>
                        <td><a href=""{referer}manage/orders/{transaction.ShoppingCartID}"">{transaction.ShoppingCartID}</a></td>
                        <td class=""text-start"">{transaction.CreditCardName}</td>
                        <td class=""text-end"">{transaction.Amount:0.00}</td>
                        <td class=""text-center"">{(transaction.Authorized == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Completed == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Voided == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Refunded == true ? "&#x2714;" : "&#x2212;")}</td>
                    </tr>");
                }
            }

            var shopIDOptions = string.Empty;
            var merchantName = string.Empty;
            var merchantEmail = string.Empty;
            var merchantSiteUrl = string.Empty;
            var checkoutType = string.Empty;
            var paymentMethodName = string.Empty;
            bool isInProduction = false;

            foreach( var shopID in merchantPropertiesList )
            {
                shopIDOptions += $"<option value='{shopID.ShopIdOption}'>{shopID.ShopIdOption}</option>";
            }

            if (merchantPropertiesList.Count > 0)
            {
                merchantName = merchantPropertiesList[0].MerchantName;
                merchantEmail = merchantPropertiesList[0].MerchantEmail;
                merchantSiteUrl = merchantPropertiesList[0].MerchantSiteUrl;
                isInProduction = merchantPropertiesList[0].IsInProduction;
                checkoutType = merchantPropertiesList[0].CheckoutType;
                paymentMethodName = merchantPropertiesList[0].PaymentMethodName;
            }

            string htmlMarkup = $@"
                <!DOCTYPE html>
                <html lang='en'>
                    {HtmlHelper.GetHtmlHeader("Order Details")}
                    {HtmlHelper.GetScriptSection(_bigCommerceConfiguration.AppUri!)}
                    <body>                       
                        <div class='m-4'>
                            {HtmlHelper.GetSitePropertiesFields(_bigCommerceConfiguration.AppUri!, jwtTokenDecoded!.Sub!, merchantName, merchantEmail, merchantSiteUrl, isInProduction, checkoutType, paymentMethodName)}
                            <hr>
                        </div>
                        <div class='m-4'>
                            <h1 class='h3'>Transaction List</h1>
                            <hr>
                            {HtmlHelper.GetForm(effectiveStartDate, effectiveEndDate, shopIDOptions, string.Empty, string.Empty, string.Empty)}
                            <table class='table table-striped'>
                                {HtmlHelper.GetTableHead()}
                                <tbody>
                                    {tableRows}
                                </tbody>
                            </table>
                        </div>
                    </body>
                </html>";

            return Content(htmlMarkup, "text/html");
        }

        [HttpGet("get-transactions")]
        public async Task<IActionResult> GetTransactions(
            DateTime? startDate, 
            DateTime? endDate, 
            string customerName,
            string shoppingCartID,
            string creditCardName,
            string shopID,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            string requestId = Guid.NewGuid().ToString();
            DateTime effectiveStartDate = startDate ?? DateTime.Today.AddDays(-10);
            DateTime effectiveEndDate = endDate ?? DateTime.Today.AddDays(1).AddMinutes(-1);

            if(string.IsNullOrEmpty(shopID))
            {
                return Ok(new List<TransactionProjection>());
            }

            // Fetch transactions based on provided dates or default to current day
            var query = new GetTransactionsQuery
            {
                CreatedFrom = effectiveStartDate,
                CreatedUntil = effectiveEndDate,
                PageSize = pageSize,
                PageNumber = pageNumber,
                CustomerName = customerName,
                ShopID = shopID,
                CreditCardName = creditCardName,
                ShoppingCartID = shoppingCartID
            };

            Logger.LogAPICommunication(Events.GetTransactionRequestReceived, new APIRequestLogState(nameof(GetTransactions), requestId, query, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            var transactions = await QueryExecutor.ExecuteAsync(query, cancellationToken);

            Logger.LogAPICommunication(Events.GetTransactionResponseReceived, new APIRequestLogState(nameof(GetTransactions), requestId, transactions, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            return Ok(transactions.ToList());
        }

        [HttpGet]
        [Route("uninstall")]
        public async Task<IActionResult> Uninstall([FromQuery] LoadRequest loadRequest, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        [Route("savemerchantproperties")]
        public async Task<IActionResult> SaveMerchantProperties([FromForm] MerchantPropertiesRequest request, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();

            var domainRequest = request.CreateDomainRequest();
            domainRequest.RequestId = requestId;

            var domainResponse = await CommandExecutor.ExecuteAsync(domainRequest, cancellationToken);

            StringBuilder tableRows = new StringBuilder();
            DateTime effectiveStartDate = DateTime.Today.AddDays(-10);
            DateTime effectiveEndDate = DateTime.Today;

            var merchantPropertiesQuery = new GetMerchantPropertiesQuery { SubjectPath = request.MerchantPath };

            var merchantPropertiesList = await QueryExecutor.ExecuteAsync(merchantPropertiesQuery, cancellationToken);

            Logger.LogAPICommunication(Events.BigCGetMerchantProp, new APIRequestLogState(nameof(Index), requestId, merchantPropertiesList, HttpContext.GetRequestHeaders(), HttpContext.GetHttpMethod(), HttpContext.GetEndpointPath()));

            // Fetch transactions based on provided dates or default to current day
            var query = new GetTransactionsQuery
            {
                CreatedFrom = effectiveStartDate,
                CreatedUntil = effectiveEndDate,
                PageSize = 10,
                PageNumber = 1,
                ShopID = string.Join("|", merchantPropertiesList.Select(_ => _.ShopIdOption))
            };

            var transactions = await QueryExecutor.ExecuteAsync(query, cancellationToken);

            var referer = string.Empty;

            if (HttpContext.GetRequestHeaders().TryGetValue("Referer", out referer))
            {

            }

            if (transactions != null && transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    tableRows.Append($@"
                    <tr>
                        <td class=""text-start"">{transaction.TransactionDateTime.ToString("dd.MM.yyyy. HH:mm")}</td>
                        <td class=""text-start"">{transaction.ShopID}</td>
                        <td class=""text-start"">{transaction.CustomerName}</td>
                        <td><a href=""{referer}manage/orders/{transaction.ShoppingCartID}"">{transaction.ShoppingCartID}</a></td>
                        <td class=""text-start"">{transaction.CreditCardName}</td>
                        <td class=""text-end"">{transaction.Amount:0.00}</td>
                        <td class=""text-center"">{(transaction.Authorized == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Completed == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Voided == true ? "&#x2714;" : "&#x2212;")}</td>
                        <td class=""text-center"">{(transaction.Refunded == true ? "&#x2714;" : "&#x2212;")}</td>
                    </tr>");
                }
            }

            var shopIDOptions = string.Empty;
            var merchantName = string.Empty;
            var merchantEmail = string.Empty;
            var merchantSiteUrl = string.Empty;
            var checkoutType = string.Empty;
            var paymentMethodName = string.Empty;
            bool isInProduction = false;

            foreach (var shopID in merchantPropertiesList)
            {
                shopIDOptions += $"<option value='{shopID.ShopIdOption}'>{shopID.ShopIdOption}</option>";
            }

            if (merchantPropertiesList.Count > 0)
            {
                merchantName = merchantPropertiesList[0].MerchantName;
                merchantEmail = merchantPropertiesList[0].MerchantEmail;
                merchantSiteUrl = merchantPropertiesList[0].MerchantSiteUrl;
                isInProduction = merchantPropertiesList[0].IsInProduction;
                checkoutType = merchantPropertiesList[0].CheckoutType;
                paymentMethodName = merchantPropertiesList[0].PaymentMethodName;
            }

            string htmlMarkup = $@"
                <!DOCTYPE html>
                <html lang='en'>
                    {HtmlHelper.GetHtmlHeader("Order Details")}
                    {HtmlHelper.GetScriptSection(_bigCommerceConfiguration.AppUri!)}
                    <body>                       
                        <div class='m-4'>
                            {HtmlHelper.GetSitePropertiesFields(_bigCommerceConfiguration.AppUri!, request.MerchantPath!, merchantName, merchantEmail, merchantSiteUrl, isInProduction, checkoutType, paymentMethodName)}
                            <hr>
                        </div>
                        <div class='m-4'>
                            <h1 class='h3'>Transaction List</h1>
                            <hr>
                            {HtmlHelper.GetForm(effectiveStartDate, effectiveEndDate, shopIDOptions, string.Empty, string.Empty, string.Empty)}
                            <table class='table table-striped'>
                                {HtmlHelper.GetTableHead()}
                                <tbody>
                                    {tableRows}
                                </tbody>
                            </table>
                        </div>
                    </body>
                </html>";

            return Content(htmlMarkup, "text/html");
        }
    }
}
