using Microsoft.Extensions.DependencyInjection.Extensions;
using UtilityCache.Interfaces;
using UtilityCache.Processors;

namespace UtilityCache;

public static class UtilityCacheModule
{
	/// <summary>
	/// Register type to container builder
	/// </summary>
	/// <param name="services"></param>
	public static void ConfigureDependencies(this IServiceCollection services) => services.TryAddSingleton<ICacheProcessor, CacheProcessor>();

}
