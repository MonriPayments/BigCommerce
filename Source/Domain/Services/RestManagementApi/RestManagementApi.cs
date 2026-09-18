using BigCommerceApi.Domain.Services.RestManagementApi.Interaction;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Text;
using System.Net.Http.Headers;

namespace BigCommerceApi.Domain.Services.RestManagementApi
{
    public sealed class RestManagementApi : IRestManagementApi
	{
		private const string GetCheckoutAction = "checkouts";
		private const string CreateOrderAction = "checkouts/{checkoutId}/orders";
		private const string UpdateOrderAction = "orders/";
		private const string CreateScriptAction = "content/scripts";
		private const string CreateWebhookAction = "hooks";
		private const string GetAccessTokenAction = "oauth2/token";

        public async Task<CreateWebhookResponse.ServiceResponse> CreateWebhookResponseAsync(CreateWebhookRequest request, string merchantApiPath, string authToken, string? requestId, CancellationToken cancellationToken)
        {
            CreateWebhookResponse.ServiceResponse? responseObject = null;

            try
            {
                string url = $"{merchantApiPath}{CreateWebhookAction}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonData = JsonConvert.SerializeObject(request);
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage res = await client.PutAsync(url, content, cancellationToken);

                    if (res.IsSuccessStatusCode)
                    {
                        string responseBody = await res.Content.ReadAsStringAsync();
                        responseObject = JsonConvert.DeserializeObject<CreateWebhookResponse.ServiceResponse>(responseBody);
                    }
                    else
                    {
                        responseObject = new CreateWebhookResponse.ServiceResponse();
                        responseObject.Error = $"StatusCode: {res.StatusCode.ToString()}";
                    }
                }

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new CreateWebhookResponse.ServiceResponse();
                responseObject.Error = $"Exception: {ex.InnerException}";

                return responseObject;
            }
        }

        public async Task<CreateScriptResponse.ServiceResponse> CreateScriptResponseAsync(CreateScriptRequest request, string merchantApiPath, string authToken, string? requestId, CancellationToken cancellationToken)
        {
			CreateScriptResponse.ServiceResponse? responseObject = null;
			try
			{
                string url = $"{merchantApiPath}{CreateScriptAction}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonData = JsonConvert.SerializeObject(request);
                    HttpContent content = new StringContent(jsonData, Encoding.Default, "application/json");
                    //HttpContent content = JsonContent.Create(request);

                    HttpResponseMessage res = await client.PostAsync(url, content, cancellationToken);

                    if (res.IsSuccessStatusCode)
                    {
                        string responseBody = await res.Content.ReadAsStringAsync();
                        responseObject = JsonConvert.DeserializeObject<CreateScriptResponse.ServiceResponse>(responseBody);
                    }
                    else
                    {
                        responseObject = new CreateScriptResponse.ServiceResponse();
                        responseObject.Error = $"StatusCode: {res.StatusCode.ToString()}";
                    }
                }

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new CreateScriptResponse.ServiceResponse();
                responseObject.Error = $"Exception: {ex.InnerException}";

                return responseObject;
            }
        }

        public async Task<CheckoutCreateOrderResponse.ServiceResponse> GetCheckoutCreateOrderResponseAsync(string merchantApiPath, string checkoutId, string authToken, string? requestId, CancellationToken cancellationToken)
		{
			CheckoutCreateOrderResponse.ServiceResponse? responseObject = null;
			try
			{
				string url = $"{merchantApiPath}{CreateOrderAction}";
				string createOrderUrl = url.Replace("{checkoutId}", checkoutId);

				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					HttpResponseMessage res = await client.PostAsync(createOrderUrl, null, cancellationToken);

					if (res.IsSuccessStatusCode)
					{
						string responseBody = await res.Content.ReadAsStringAsync();
						responseObject = JsonConvert.DeserializeObject<CheckoutCreateOrderResponse.ServiceResponse>(responseBody);
					}
					else
					{
                        string responseBody = await res.Content.ReadAsStringAsync();
                        responseObject = new CheckoutCreateOrderResponse.ServiceResponse();
						responseObject.Error = $"StatusCode: {res.StatusCode}, Message: {responseBody}";
					}
				}

				return responseObject;
			}
			catch (Exception ex)
			{
				responseObject = new CheckoutCreateOrderResponse.ServiceResponse();
				responseObject.Error = $"Exception: {ex}, InnerEx: {ex.InnerException}";

				return responseObject;
			}
		}

		public async Task<GetCheckoutResponse.ServiceResponse> GetCheckoutResponseAsync(string merchantApiPath, string checkoutId, string authToken, string? requestId, CancellationToken cancellationToken)
		{
			GetCheckoutResponse.ServiceResponse? responseObject = null;
			try
			{		
				string url = $"{merchantApiPath}{GetCheckoutAction}/{checkoutId}";

				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);

					HttpResponseMessage res = await client.GetAsync(url, cancellationToken);

					if (res.IsSuccessStatusCode)
					{
						string responseBody = await res.Content.ReadAsStringAsync();
						responseObject = JsonConvert.DeserializeObject<GetCheckoutResponse.ServiceResponse>(responseBody);
					}
					else
					{
						responseObject = new GetCheckoutResponse.ServiceResponse();
						responseObject.Error = $"StatusCode: {res.StatusCode.ToString()}";
					}
				}

				return responseObject;
			}
			catch(Exception ex)
			{
				responseObject = new GetCheckoutResponse.ServiceResponse();
				responseObject.Error = $"Exception: {ex.InnerException}";

				return responseObject;
			}
		}

		public async Task<UpdateOrderResponse.ServiceResponse> UpdateOrderResponseAsync(UpdateOrderRequest request, string merchantApiPath, string orderId, string authToken, string? requestId, CancellationToken cancellationToken)
		{
			UpdateOrderResponse.ServiceResponse? responseObject = null;
			try
			{
				string url = $"{merchantApiPath}{UpdateOrderAction}{orderId}";
				url = url.Replace("v3", "v2");
				//url = url.Replace("{order_id}", orderId);

				using (HttpClient client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					string jsonData = JsonConvert.SerializeObject(request);
					HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

					HttpResponseMessage res = await client.PutAsync(url, content, cancellationToken);

					if (res.IsSuccessStatusCode)
					{
						string responseBody = await res.Content.ReadAsStringAsync();
						responseObject = JsonConvert.DeserializeObject<UpdateOrderResponse.ServiceResponse>(responseBody);
					}
					else
					{
						responseObject = new UpdateOrderResponse.ServiceResponse();
						responseObject.Error = $"StatusCode: {res.StatusCode}, Content: {res.Content}, ReasonPhrase: {res.ReasonPhrase}";
					}
				}

				return responseObject;
			}
			catch (Exception ex)
			{
				responseObject = new UpdateOrderResponse.ServiceResponse();
				responseObject.Error = $"Exception: {ex.InnerException}";

				return responseObject;
			}
		}

        public async Task<GetAccessTokenResponse.ServiceResponse> GetAccessTokenResponseAsync(GetAccessTokenRequest request, string authPath, string? requestId, CancellationToken cancellationToken)
        {
            GetAccessTokenResponse.ServiceResponse? responseObject = null;

            try
            {
                string url = $"{authPath}{GetAccessTokenAction}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonData = JsonConvert.SerializeObject(request);
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage res = await client.PostAsync(url, content, cancellationToken);

                    if (res.IsSuccessStatusCode)
                    {
                        string responseBody = await res.Content.ReadAsStringAsync();
                        responseObject = JsonConvert.DeserializeObject<GetAccessTokenResponse.ServiceResponse>(responseBody);
                    }
                    else
                    {
                        responseObject = new GetAccessTokenResponse.ServiceResponse();
                        responseObject.Error = $"StatusCode: {res.StatusCode.ToString()}";
                    }
                }

                return responseObject;
            }
            catch (Exception ex)
            {
                responseObject = new GetAccessTokenResponse.ServiceResponse();
                responseObject.Error = $"Exception: {ex.InnerException}";

                return responseObject;
            }
        }

    }
}
