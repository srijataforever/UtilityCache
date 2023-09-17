using UtilityCache.Models;

namespace UtilityCache.Tests
{
	public static class TestData
	{
		public static LruCacheItem<string, object> CacheItem1() => new LruCacheItem<string, object>("1", "string");

		public static string JSON() => "[{ \"firmano\":128257,\"adi\":\"- FATİH YILMAZ\"},{ \"firmano\":128446,\"adi\":\"-MEHMET ÜSTÜN\"}]";
	}
}
