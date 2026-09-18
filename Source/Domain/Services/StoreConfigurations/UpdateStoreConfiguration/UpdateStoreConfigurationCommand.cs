using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Services.StoreConfigurations.AddShopToStoreConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.StoreConfigurations.EditStoreConfiguration
{
    public class UpdateStoreConfigurationCommand : ICommand<Response<UpdateStoreConfigurationResult>>
    {
        public string Id { get; set; }
        public string? AccessToken { get; set; }
        public string? ClientName { get; set; }
        public string? ClientID { get; set; }
        public string? ClientSecret { get; set; }
        public string? StoreApiAccountName { get; set; }
        public string? ApiPath { get; set; }
        public string? MerchantSiteUrl { get; set; }
        public string? SubjectPath { get; set; }
        public string? MerchantEmail { get; set; }
        public string? MerchantName { get; set; }
        public string? PaymentMethodName { get; set; }
        public StoreStatus? StoreStatus { get; set; }
        public CheckoutType? CheckoutType { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public string? PaymentConfiguration { get; set; }
        public string? OrderConfirmationRedirectRoute { get; set; }
        public OrderStatus? DefaultOrderStatus { get; set; }
        public Guid? IdShop { get; set; }
    }
}
