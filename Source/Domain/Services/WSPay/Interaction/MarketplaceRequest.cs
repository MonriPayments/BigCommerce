using Newtonsoft.Json;
using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public class MarketplaceRequest
    {
        [JsonProperty("Transactions")]
        public List<RelatedTransactionRequest>? RelatedTransactions { get; set; }
    }
}
