using Microsoft.Extensions.Caching.Memory;

namespace BigCommerceApi.Domain.Services.Cache
{
	public interface IWebAppCache : IMemoryCache
	{
		double? Timeout { get; }
	}
}
