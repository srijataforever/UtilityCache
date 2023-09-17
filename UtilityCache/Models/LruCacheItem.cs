using System.ComponentModel.DataAnnotations;

namespace UtilityCache.Models
{
	public class LruCacheItem<TKey, TValue>
	{
		public LruCacheItem(TKey k, TValue v)
		{
			Key = k;
			Value = v;
		}

		[Required]
		public TKey Key { get; }

		[Required]
		public TValue Value { get; }
	}
}
