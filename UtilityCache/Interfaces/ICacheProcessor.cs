namespace UtilityCache.Interfaces;

public interface ICacheProcessor
{
	Task<bool> Create<TItem>(string key, TItem value, CancellationToken token = default);
	Task<TItem> Fetch<TItem>(string key, CancellationToken token = default);
}