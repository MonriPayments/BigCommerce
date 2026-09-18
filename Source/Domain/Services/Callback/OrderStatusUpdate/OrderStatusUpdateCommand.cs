using BigCommerceApi.Domain.Model.Stores;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Callback.OrderStatusUpdate
{
    public class OrderStatusUpdateCommand : ICommand<Response<OrderStatusUpdateResult?>>
    {
        public int? Id { get; set; }
        public string? Scope { get; set; }
        public string? Type { get; set; }
        public OrderStatus? PreviousStatusId { get; set; }
        public OrderStatus? NewStatusId { get; set; }
        public string? StoreId { get; set; }
        public string? Hash { get; set; }
        public int? CreatedAt { get; set; }
        public string? Producer { get; set; }
        public string? RequestId { get; set; }
    }
}
