using UtilityCache.Models;

namespace UtilityCache.Helpers
{
	public static class LruCacheHelper<TKey, TValue>
	{
		private static IDictionary<TKey, LinkedListNode<LruCacheItem<TKey, TValue>>> cacheMap = new Dictionary<TKey, LinkedListNode<LruCacheItem<TKey, TValue>>>();
		private static LinkedList<LruCacheItem<TKey, TValue>> lruList = new LinkedList<LruCacheItem<TKey, TValue>>();

		public static Task<TValue> Get(TKey key, CancellationToken token = default)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (!cacheMap.ContainsKey(key))
			{
				throw new InvalidOperationException($"Key '{key}' does not exist in cache");
			}

			var value = cacheMap.FirstOrDefault(k => k.Key != null && k.Key.Equals(key)).Value.Value.Value;

			lock (cacheMap)
			{
				if (cacheMap.TryGetValue(key, out var node))
				{
					value = node.Value.Value;
					lruList.Remove(node);
					lruList.AddLast(node);
				}
			}

			return Task.FromResult(value);
		}

		public static Task<bool> Set(TKey key, TValue value, int capacity, CancellationToken token = default)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (cacheMap.Count > 0 && cacheMap.ContainsKey(key))
			{
				throw new InvalidOperationException($"Key '{key}' already exists in cache");
				//maybe update
			}

			lock (cacheMap)
			{
				if (cacheMap.Count >= capacity)
				{
					var delNode = lruList.First;
					lruList.RemoveFirst();
					cacheMap.Remove(delNode.Value.Key);
				}

				var cacheItem = new LruCacheItem<TKey, TValue>(key, value);
				var node = new LinkedListNode<LruCacheItem<TKey, TValue>>(cacheItem);
				lruList.AddLast(node);
				cacheMap.Add(key, node);
			}

			return Task.FromResult(cacheMap.ContainsKey(key));
		}
	}
}
