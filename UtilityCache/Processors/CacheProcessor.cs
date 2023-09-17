using UtilityCache.Helpers;
using UtilityCache.Interfaces;

namespace UtilityCache.Processors
{
	public sealed class CacheProcessor : ICacheProcessor
	{
		private readonly IConfiguration configuration;

		public CacheProcessor(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public Task<bool> Create<TItem>(string key, TItem value, CancellationToken token = default)
		{
			var capacity = configuration.GetValue<int>("Capacity");
			return LruCacheHelper<string, TItem>.Set(key, value, capacity, token);
		}

		public Task<TItem> Fetch<TItem>(string key, CancellationToken token = default) => LruCacheHelper<string, TItem>.Get(key, token);
	}
}
