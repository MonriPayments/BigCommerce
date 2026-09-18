using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
	public class UpdateOrderResponse : RestManagementApiResponse<UpdateOrderResponse.ServiceResponse>
	{
		public UpdateOrderResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
		{

		}

		public class ServiceResponse
		{
			public string? Error { get; set; }
			[JsonProperty("shipping_address")]
			public ShippingAddress? ShippingAddress { get; set; }

			[JsonProperty("billing_address")]
			public BillingAddress? BillingAddress { get; set; }
			public Coupons? Coupons { get; set; }
			public Consignments? Consignments { get; set; }
			public Products? Products { get; set; }
			public int? Id { get; set; }

			[JsonProperty("date_modified")]
			public string? DateModified { get; set; }

			[JsonProperty("date_shipped")]
			public string? DateShipped { get; set; }

			[JsonProperty("cart_id")]
			public string? CartId { get; set; }

			public string? Status { get; set; }

			[JsonProperty("subtotal_tax")]
			public string? SubtotalTax { get; set; }

			[JsonProperty("shipping_cost_tax")]
			public string? ShippingCostTax { get; set; }

			[JsonProperty("shipping_cost_tax_class_id")]
			public int? ShippingCostTaxClassId { get; set; }

			[JsonProperty("handling_cost_tax")]
			public string? HandlingCostTax { get; set; }

			[JsonProperty("handling_cost_tax_class_id")]
			public int? HandlingCostTaxClassId { get; set; }

			[JsonProperty("wrapping_cost_tax")]
			public string? WrappingCostTax { get; set; }

			[JsonProperty("wrapping_cost_tax_class_id")]
			public int? WrappingCostTaxClassId { get; set; }

			[JsonProperty("payment_status")]
			public string? PaymentStatus { get; set; }

			[JsonProperty("store_credit_amount")]
			public string? StoreCreditAmount { get; set; }

			[JsonProperty("gift_certificate_amount")]
			public string? GiftCertificateAmount { get; set; }

			[JsonProperty("currency_id")]
			public int? CurrencyId { get; set; }

			[JsonProperty("currency_code")]
			public string? CurrencyCode { get; set; }

			[JsonProperty("currency_exchange_rate")]
			public string? CurrencyExchangeRate { get; set; }

			[JsonProperty("default_currency_id")]
			public int? DefaultCurrencyId { get; set; }

			[JsonProperty("default_currency_code")]
			public string? DefaultCurrencyCode { get; set; }

			[JsonProperty("store_default_currency_code")]
			public string? StoreDefaultCurrencyCode { get; set; }

			[JsonProperty("store_default_to_transactional_exchange_rate")]
			public string? StoreDefaultToTransactionalExchangeRate { get; set; }

			[JsonProperty("coupon_discount")]
			public string? CouponDiscount { get; set; }

			[JsonProperty("shipping_address_count")]
			public int? ShippingAddressCount { get; set; }

			[JsonProperty("is_email_opt_in")]
			public bool? IsEmailOptIn { get; set; }

			[JsonProperty("order_source")]
			public string? OrderSource { get; set; }

			[JsonProperty("status_id")]
			public int? StatusId { get; set; }

			[JsonProperty("base_handling_cost")]
			public string? BaseHandlingCost { get; set; }

			[JsonProperty("base_shipping_cost")]
			public string? BaseShippingCost { get; set; }

			[JsonProperty("base_wrapping_cost")]
			public string? BaseWrappingCost { get; set; }

			[JsonProperty("channel_id")]
			public int? ChannelId { get; set; }

			[JsonProperty("customer_id")]
			public int? CustomerId { get; set; }

			[JsonProperty("customer_message")]
			public string? CustomerMessage { get; set; }

			[JsonProperty("date_created")]
			public string? DateCreated { get; set; }

			[JsonProperty("discount_amount")]
			public string? DiscountAmount { get; set; }

			[JsonProperty("ebay_order_id")]
			public string? EbayOrderId { get; set; }

			[JsonProperty("external_id")]
			public string? ExternalId { get; set; }

			[JsonProperty("external_merchant_id")]
			public string? ExternalMerchantId { get; set; }

			[JsonProperty("external_source")]
			public string? ExternalSource { get; set; }

			[JsonProperty("geoip_country")]
			public string? GeoipCountry { get; set; }

			[JsonProperty("geoip_country_iso2")]
			public string? GeoipCountryIso2 { get; set; }

			[JsonProperty("handling_cost_ex_tax")]
			public string? HandlingCostExTax { get; set; }

			[JsonProperty("handling_cost_inc_tax")]
			public string? HandlingCostIncTax { get; set; }

			[JsonProperty("ip_address")]
			public string? IpAddress { get; set; }

			[JsonProperty("ip_address_v6")]
			public string? IpAddressV6 { get; set; }

			[JsonProperty("is_deleted")]
			public bool? IsDeleted { get; set; }

			[JsonProperty("items_shipped")]
			public int? ItemsShipped { get; set; }

			[JsonProperty("items_total")]
			public int? ItemsTotal { get; set; }

			[JsonProperty("order_is_digital")]
			public bool? OrderIsDigital { get; set; }

			[JsonProperty("payment_method")]
			public string? PaymentMethod { get; set; }

			[JsonProperty("refunded_amount")]
			public string? RefundedAmount { get; set; }

			[JsonProperty("shipping_cost_ex_tax")]
			public string? ShippingCostExTax { get; set; }

			[JsonProperty("shipping_cost_inc_tax")]
			public string? ShippingCostIncTax { get; set; }

			[JsonProperty("staff_notes")]
			public string? StaffNotes { get; set; }

			[JsonProperty("subtotal_ex_tax")]
			public string? SubtotalExTax { get; set; }

			[JsonProperty("subtotal_inc_tax")]
			public string? SubtotalIncTax { get; set; }

			[JsonProperty("tax_provider_id")]
			public string? TaxProviderId { get; set; }

			[JsonProperty("customer_localce")]
			public string? CustomerLocale { get; set; }

			[JsonProperty("external_order_id")]
			public string? ExternalOrderId { get; set; }

			[JsonProperty("total_ex_tax")]
			public string? TotalExTax { get; set; }

			[JsonProperty("total_inc_tax")]
			public string? TotalIncTax { get; set; }

			[JsonProperty("wrapping_cost_ex_tax")]
			public string? WrappingCostExTax { get; set; }

			[JsonProperty("wrapping_cost_inc_tax")]
			public string? WrappingCostIncTax { get; set; }
		}

		public class BillingAddress
		{

			[JsonProperty("form_fields")]
			public List<FormFields>? FormFields { get; set; }

			[JsonProperty("first_name")]
			public string? FirstName { get; set; }

			[JsonProperty("last_name")]
			public string? LastName { get; set; }

			[JsonProperty("street_1")]
			public string? Street1 { get; set; }

			[JsonProperty("street_2")]
			public string? Street2 { get; set; }

			[JsonProperty("country_iso_2")]
			public string? CountryIso2 { get; set; }
			public string? Company { get; set; }
			public string? State { get; set; }
			public string? Zip { get; set; }
			public string? Country { get; set; }
			public string? City { get; set; }
			public string? Phone { get; set; }

			public string? Email { get; set; }				
		}

		public class Coupons
		{
			public string? Url { get; set; }
			public string? Resource { get; set; }
		}

		public class FormFields
		{
			public string? Name { get; set; }
			public string? Value { get; set; }
		}

		public class ShippingAddress
		{
			public string? Url { get; set; }
			public string? Resource { get; set; }
		}

		public class Products
		{
			public string? Url { get; set; }
			public string? Resource { get; set; }
		}

		public class Consignments
		{
			public string? Url { get; set; }
			public string? Resource { get; set; }
		}
	}
}
