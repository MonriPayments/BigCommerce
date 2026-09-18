using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BigCommerceApi.Domain.Services.RestManagementApi.Interaction
{
	public sealed class GetCheckoutResponse : RestManagementApiResponse<GetCheckoutResponse.ServiceResponse>
	{
        public GetCheckoutResponse(ServiceResponse? serviceResponse = null, Exception? raisedException = null) : base(serviceResponse, raisedException)
		{
            
        }


        public sealed class ServiceResponse
        {
            public Data? Data { get; set; }
            public object? Metadata { get; set; }
			public string? Error { get; set; }
        }

        public sealed class Data
        {
            public string? Id { get; set; }

            [JsonProperty("Cart")]
            public Cart? Cart { get; set; }
			public List<Consignments>? Consignments { get; set; }

			[JsonProperty("billing_address")]
			public BillingAddress? BillingAddress { get; set; }
			public List<Taxes>? Taxes { get; set; }
			public List<Coupons>? Coupons { get; set; }
			public List<Promotions>? Promotions { get; set; }

			[JsonProperty("order_id")]
			public string? OrderId { get; set; }

			[JsonProperty("shipping_cost_total_inc_tax")]
			public double? ShippingCostTotalIncTax { get; set; }

			[JsonProperty("shipping_cost_total_ex_tax")]
			public double? ShippingCostTotalExTax { get; set; }

			[JsonProperty("handling_cost_total_inc_tax")]
			public double? HandlingCostTotalIncTax { get; set; }

			[JsonProperty("handling_cost_total_ex_tax")]
			public double? HandlingCostTotalExTax { get; set; }

			[JsonProperty("tax_total")]
			public double? TaxTotal { get; set; }

			[JsonProperty("subtotal_inc_tax")]
			public double? SubtotalIncTax { get; set; }

			[JsonProperty("subtotal_ex_tax")]
			public double? SubtotalExTax { get; set; }

			[JsonProperty("grand_total")]
			public double? GrandTotal { get; set; }

			[JsonProperty("created_time")]
			public string? CreatedTime { get; set; }

			[JsonProperty("updated_time")]
			public string? UpdatedTime { get; set; }

			[JsonProperty("customer_message")]
			public string? CustomerMessage { get; set; }
		}

        public sealed class Cart
        {
            [JsonProperty("Currency")]
            public Currency? Currency { get; set; }
			public List<Coupons>? Coupons { get; set; }
			public List<Discounts>? Discounts { get; set; }
			[JsonProperty("line_items")]
			public LineItems? LineItems { get; set; }
			public string? Id { get; set; }
			public string? Email { get; set; }

			[JsonProperty("customer_id")]
			public string? CustomerId { get; set; }

			[JsonProperty("base_amount")]
			public double? BaseAmount { get; set; }

			[JsonProperty("channel_id")]
			public int? ChannelId { get; set; }

			[JsonProperty("discount_amount")]
			public double? DiscountAMount { get; set; }

			[JsonProperty("cart_amount_inc_tax")]
			public double? CartAmountIncTax { get; set; }

			[JsonProperty("cart_amount_ex_tax")]
			public double? CartAmountExTax { get; set; }

			[JsonProperty("created_time")]
			public string? CreatedTime { get; set; }

			[JsonProperty("updated_time")]
			public string? UpdatedTime { get; set; }
		}

        public sealed class Currency
        {
            [JsonProperty("Code")]
            public string? Code { get; set; }
        }

        public sealed class Coupons
        {
            public string? Code { get; set; }
            public int? Id { get; set; }

            [JsonProperty("coupon_type")]
            public string? CouponType { get; set; }

			[JsonProperty("discounted_amount")]
			public double? DiscountedAmount { get; set; }
        }

		public sealed class LineItems
        {
			[JsonProperty("physical_items")]
			public List<PhysicalItems>? PhysicalItems { get; set; }

			[JsonProperty("digital_items")]
			public List<DigitalItems>? DigitalItems { get; set; }

			[JsonProperty("gift_items")]
			public List<GiftCertificates>? GiftCertificates { get; set; }

			[JsonProperty("custom_items")]
			public List<CustomItems>? CustomItems { get; set; }
        }

        public sealed class PhysicalItems
        {
			[JsonProperty("gift_wrapping")]
			public GiftWrapping? GiftWrapping { get; set; }
			public List<Discounts>? Discounts { get; set; }
			public string? Id { get; set; }
			public int? Quantity { get; set; }
            public string? Sku { get; set; }
            public string? Name { get; set; }
            public string? Url { get; set; }

			[JsonProperty("variant_id")]
			public int? VariantId { get; set; }

			[JsonProperty("product_id")]
			public int? ProductId { get; set; }

			[JsonProperty("is_taxable")]
			public bool? IsTaxable { get; set; }

			[JsonProperty("image_url")]
			public string? ImageUrl { get; set; }

			[JsonProperty("discount_amount")]
			public double? DiscountAmount { get; set; }

			[JsonProperty("coupon_amount")]
			public double? CouponAmount { get; set; }

			[JsonProperty("original_price")]
			public double? OriginalPrice { get; set; }

			[JsonProperty("list_price")]
			public double? ListPrice { get; set; }

			[JsonProperty("sale_price")]
			public double? SalePrice { get; set; }

			[JsonProperty("extended_list_price")]
			public double? ExtendedListPrice { get; set; }

			[JsonProperty("extended_sale_price")]
			public double? ExtendedSalePrice { get; set; }

			[JsonProperty("is_require_shipping")]
			public bool? IsRequireShipping { get; set; }

			[JsonProperty("is_mutable")]
			public bool? IsMutable { get; set; }

			[JsonProperty("parent_id")]
			public int? ParentId { get; set; }            
		}

		public sealed class DigitalItems
		{
			public List<Discounts>? Discounts { get; set; }
			public string? Id { get; set; }
			public int? Quantity { get; set; }
			public string? Sku { get; set; }
			public string? Name { get; set; }
			public string? Url { get; set; }

			[JsonProperty("is_mutable")]
			public bool? IsMutable { get; set; }

			[JsonProperty("is_require_shipping")]
			public bool? IsRequireShipping { get; set; }

			[JsonProperty("is_taxable")]
			public bool? IsTaxable { get; set; }

			[JsonProperty("image_url")]
			public string? ImageUrl { get; set; }

			[JsonProperty("discount_amount")]
			public double? DiscountAmount { get; set; }

			[JsonProperty("coupon_amount")]
			public double? CouponAmount { get; set; }

			[JsonProperty("original_price")]
			public double? OriginalPrice { get; set; }

			[JsonProperty("list_price")]
			public double? ListPrice { get; set; }

			[JsonProperty("sale_price")]
			public double? SalePrice { get; set; }

			[JsonProperty("extended_list_price")]
			public double? ExtendedListPrice { get; set; }

			[JsonProperty("extended_sale_price")]
			public double? ExtendedSalePrice { get; set; }

			[JsonProperty("variant_id")]
			public int? VariantId { get; set; }

			[JsonProperty("parent_id")]
			public int? ParentId { get; set; }

			[JsonProperty("product_id")]
			public int? ProductId { get; set; }

		}

		public sealed class Consignments
		{
			public Address? Address { get; set; }

			[JsonProperty("available_shipping_options")]
			public List<AvailableShippingOptions>? AvailableShippingOptions { get; set; }

			[JsonProperty("selected_shipping_option")]
			public SelectedShippingOption? SelectedShippingOption { get; set; }

			[JsonProperty("coupon_discounts")]
			public List<CouponDiscounts>? CouponDiscounts { get; set; }
			public List<Discounts>? Discounts { get; set; }
			public SelectedPickupOption? SelectedPickupOption { get; set; }
			public string? Id { get; set; }

			[JsonProperty("shipping_address")]
			public object? ShipppingAddress { get; set; }

			[JsonProperty("shipping_cost_total_inc_tax")]
			public double? ShippingCostTotalIncTax { get; set; }

			[JsonProperty("shipping_cost_total_ex_tax")]
			public double? ShippingCostTotalExTax { get; set; }

			[JsonProperty("handling_cost_total_inc_tax")]
			public double? HandlingCostTotalIncTax { get; set; }

			[JsonProperty("handling_cost_total_ex_tax")]
			public double? HandlingCostTotalExTax { get; set; }

			[JsonProperty("line_item_ids")]
			public List<string>? LineItemIds { get; set; }
		}

		public sealed class AvailableShippingOptions
		{
			public string? Id { get; set; }
			public string? Description { get; set; }
			public string? Type { get; set; }
			public double? Cost { get; set; }

			[JsonProperty("image_url")]
			public string? ImageUrl { get; set; }

			[JsonProperty("transit_time")]
			public string? TransitTime { get; set; }

			[JsonProperty("additional_description")]
			public string? AdditionalDescription { get; set; }
		}

		public sealed class SelectedShippingOption
		{
			public string? Id { get; set; }
			public string? Description { get; set; }
			public string? Type { get; set; }
			public double? Cost { get; set; }

			[JsonProperty("image_url")]
			public string? ImageUrl { get; set; }

			[JsonProperty("transit_time")]
			public string? TransitTime { get; set; }

			[JsonProperty("additional_description")]
			public string? AdditionalDescription { get; set; }
		}

		public sealed class Address
		{
			public List<CustomFields>? CustomFields { get; set; }
			public string? Id { get; set; }
			public string? Email { get; set; }
			public string? Company { get; set; }
			public string? Address1 { get; set; }
			public string? Address2 { get; set; }
			public string? City { get; set; }
			public string? Phone { get; set; }

			[JsonProperty("first_name")]
			public string? FirstName { get; set; }

			[JsonProperty("last_name")]
			public string? LastName { get; set; }

			[JsonProperty("state_or_province")]
			public string? StateOrProvince { get; set; }

			[JsonProperty("state_or_province_code")]
			public string? StateOrProvinceCode { get; set; }

			[JsonProperty("country_code")]
			public string? CountryCode { get; set; }

			[JsonProperty("postal_code")]
			public string? PostalCode { get; set; }
		}

		public sealed class GiftCertificates
		{
			public string? Id { get; set; }
			public string? Theme { get; set; }
			public double? Amount { get; set; }
			public string? Name { get; set; }
			public bool? Taxable { get; set; }
			public string? Message { get; set; }
		}

		public sealed class GiftWrapping
        {
			public Sender? Sender { get; set; }
			public Recipient? Recipient { get; set; }
			public string? Name { get; set; }
			public string? Message { get; set; }
			public double? Amount { get; set; }

			[JsonProperty("amount_as_integer")]
			public int? AmountAsInteger { get; set; }
        }

		public sealed class CustomItems
		{
			public string? Id { get; set; }
			public int? Quantity { get; set; }
			public string? Sku { get; set; }
			public string? Name { get; set; }

			[JsonProperty("extended_list_price")]
			public double? ExtendedListPrice { get; set; }

			[JsonProperty("list_price")]
			public double? ListPrice { get; set; }

			[JsonProperty("image_url")]
			public string? ImageUrl { get; set; }
		}

		public sealed class BillingAddress
		{
			[JsonProperty("custom_fields")]
			public List<CustomFields>? CustomFields { get; set; }
			public string? Id { get; set; }
			public string? Email { get; set; }
			public string? Company { get; set; }
			public string? Address1 { get; set; }
			public string? Address2 { get; set; }
			public string? City { get; set; }
			public string? Phone { get; set; }

			[JsonProperty("first_name")]
			public string? FirstName { get; set; }

			[JsonProperty("last_name")]
			public string? LastName { get; set; }

			[JsonProperty("state_or_province")]
			public string? StateOrProvince { get; set; }

			[JsonProperty("state_or_province_code")]
			public string? StateOrProvinceCode { get; set; }

			[JsonProperty("country_code")]
			public string? CountryCode { get; set; }

			[JsonProperty("postal_code")]
			public string? PostalCode { get; set; }
		}

		public sealed class CouponDiscounts
		{
			public string? Code { get; set; }
			public double? Amount { get; set; }
		}


		public sealed class Discounts
        {
            public string? Id { get; set; }

			[JsonProperty("discounted_amount")]
			public double? DiscountedAmount { get; set; }
		}

		public sealed class Promotions
		{
			public List<Banners>? Banners { get; set; }
		}

		public sealed class Banners
		{
			public List<string>? Page { get; set; }
			public string? Id { get; set; }
			public string? Type { get; set; }
			public string? Text { get; set; }
		}

		public sealed class Sender
		{
			public string? Name { get; set; }
			public string? Email { get; set; }
		}

		public sealed class Recipient
		{
			public string? Name { get; set; }
			public string? Email { get; set; }
		}

		public sealed class Taxes
		{
			public string? Name { get; set; }
			public double? Amount { get; set; }
		}

		public sealed class SelectedPickupOption
		{
			[JsonProperty("pickup_method_id")]
			public int? PickupMethodId { get; set; }
		}

		public sealed class CustomFields
		{
			[JsonProperty("field_id")]
			public string? FieldId { get; set; }

			[JsonProperty("field_value")]
			public string? FieldValue { get; set; }
		}
	}
}
