using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.Callback.OrderStatusUpdate;
using Newtonsoft.Json;

namespace BigCommerceApi.Client.Web.Interaction.Requests
{
    public class OrderStatusUpdateRequest : BaseAPIRequest<OrderStatusUpdateCommand>
    {
        public Data? Data { get; set; }
        public string? Scope { get; set; }
        public string? Hash { get; set; }
        public string? Producer { get; set; }

        [JsonProperty("store_id")]
        public string? StoreId { get; set; }

        [JsonProperty("created_at")]
        public int? CreatedAt { get; set; }

        public string? Error { get; set; }

        protected override void PopulateDomainRequest(OrderStatusUpdateCommand domainRequest)
        {
            domainRequest.Producer = Producer;
            domainRequest.Scope = Scope;
            domainRequest.Hash = Hash;
            domainRequest.StoreId = StoreId;
            domainRequest.CreatedAt = CreatedAt;
            domainRequest.Type = Data?.Type;
            domainRequest.Id = Data?.Id;
            domainRequest.PreviousStatusId = (OrderStatus)Data?.Status?.PreviousStatusId!;
            domainRequest.NewStatusId = (OrderStatus)Data?.Status?.NewStatusId!;
        }
    }

    public class Data
    {
        public int? Id { get; set; }
        public string? Type { get; set; }

        public Status? Status { get; set; }
    }

    public class Status
    {
        [JsonProperty("previous_status_id")]
        public int? PreviousStatusId { get; set; }

        [JsonProperty("new_status_id")]
        public int? NewStatusId { get; set; }
    }
}
