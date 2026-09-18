using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Orders
{
	public class UpdateOrderCommand : ICommand<Response<UpdateOrderResult>>
	{
		public string? OrderId { get; set; }
		public string? ShopId { get; set; }
	}
}
