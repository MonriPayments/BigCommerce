using BigCommerceApi.Domain.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCommerceApi.Domain.Projections.StoreConfigurations
{
    public class GetStoreConfigurationsProjection
    {
        public List<StoreConfigurationDto> StoreConfigurations { get; set; }
    }

    public class StoreConfigurationDto
    {
        public Guid Id { get; set; }
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
        public string? StoreStatus { get; set; }
        public string? CheckoutType { get; set; }
        public string? PaymentGateway { get; set; }
        public string? PaymentConfiguration { get; set; }
        public string? OrderConfirmationRedirectRoute { get; set; }
        public string? DefaultOrderStatus { get; set; }
        public Guid? IdShop { get; set; }
    }
}
