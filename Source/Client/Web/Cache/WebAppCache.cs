using BigCommerceApi.Domain.Services.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BigCommerceApi.Client.Web.Cache
{
	public class WebAppCache : MemoryCache, IWebAppCache
	{
		private readonly int _defaultCacheTimeout = 30;
		public double? Timeout { get; set; }

		public WebAppCache(IOptions<MemoryCacheOptions> optionsAccessor) : base(optionsAccessor)
		{
			Timeout = _defaultCacheTimeout;
		}

		public ICacheEntry CreateEntry(object key)
		{
			return base.CreateEntry(key);
		}

		public void Remove(object key)
		{
			base.Remove(key);
		}

		public bool TryGetValue(object key, out object value)
		{
			return base.TryGetValue(key, out value);
		}

		public void Dispose()
		{
			base.Dispose();
		}
	}
}
