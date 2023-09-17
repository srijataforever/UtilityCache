using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using UtilityCache.Processors;

namespace UtilityCache.Tests
{
	[ExcludeFromCodeCoverage]
	[TestFixture]
	public class CacheProcessorTest
	{
		private CacheProcessor cacheProcessor;
		private IConfigurationRoot config;

		[OneTimeSetUp]
		public void Init()
		{
			config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
		}

		[SetUp]
		public void InitializeCacheProcessor() => cacheProcessor = new CacheProcessor(config);

		[Test]
		public async Task Create_200_Success()
		{
			var result = await cacheProcessor.Create("1", "string").ConfigureAwait(false);
			Assert.That(result, Is.EqualTo(true));
		}

		[Test]
		public async Task Create_200_Success_Lru()
		{
			Parallel.Invoke(async () => await cacheProcessor.Create("1", "string").ConfigureAwait(false),
				async () => await cacheProcessor.Create("2", "1").ConfigureAwait(false),
				async () => await cacheProcessor.Create("3", "2.5").ConfigureAwait(false),
				async () => await cacheProcessor.Create("4", TestData.JSON()).ConfigureAwait(false),
				async () => await cacheProcessor.Create("5", "1L").ConfigureAwait(false));
			var result = await cacheProcessor.Create("6", "last node is removed").ConfigureAwait(false);
			Assert.That(result, Is.EqualTo(true));
		}

		[Test]
		public async Task Create_400_Fail_KeyExists()
		{
			await cacheProcessor.Create("1", "string").ConfigureAwait(false);
			Assert.That(async () => await cacheProcessor.Create("1", "string").ConfigureAwait(false),
				Throws.Exception.TypeOf<InvalidOperationException>().With.Property("Message").EqualTo("Key '1' already exists in cache"));
		}

		[Test]
		public void Create_400_Fail_NullParameter() => Assert.That(async () => await cacheProcessor.Create(null, "string").ConfigureAwait(false), Throws.Exception.TypeOf<ArgumentNullException>().With.Property("Message").EqualTo("Value cannot be null. (Parameter 'key')"));

		// [Test]
		// public void Fetch_200_Success()
		// {
		// 	cacheProcessor.Create("1", "string");
		// 	var result = cacheProcessor.Fetch<string>("1");
		// 	Assert.That(result.Result, Is.EqualTo("string"));
		// }

		// [Test]
		// public void Fetch_400_Fail_KeyDoesNotExist() => Assert.That(async () => await cacheProcessor.Fetch<string>("1").ConfigureAwait(false), Throws.Exception.TypeOf<InvalidOperationException>().With.Property("Message").EqualTo("Key '1' does not exist in cache"));

		[Test]
		public void Fetch_400_Fail_NullParameter() => Assert.That(async () => await cacheProcessor.Fetch<string>(null).ConfigureAwait(false), Throws.Exception.TypeOf<ArgumentNullException>().With.Property("Message").EqualTo("Value cannot be null. (Parameter 'key')"));
	}
}