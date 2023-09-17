using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using UtilityCache.Controllers;
using UtilityCache.Interfaces;

namespace UtilityCache.Tests
{
	[ExcludeFromCodeCoverage]
	[TestFixture]
	public class CacheControllerTest
	{
		private static CacheController cacheController;
		private Mock<ICacheProcessor> cacheProcessorMock;

		[SetUp]
		public void Init()
		{
			cacheProcessorMock = new Mock<ICacheProcessor>(MockBehavior.Default);
			cacheController = new CacheController(cacheProcessorMock.Object);
		}

		[Test]
		public async Task Create_200_Success()
		{
			cacheProcessorMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>(), default)).Returns(Task.FromResult(true));
			var iActionResult = await cacheController.Create("1", "string").ConfigureAwait(false);
			var result = iActionResult as OkResult;
			Assert.That(result?.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public async Task Create_400_Fail()
		{
			cacheProcessorMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>(), default)).Returns(Task.FromResult(false));
			var iActionResult = await cacheController.Create("1", "string").ConfigureAwait(false);
			var result = iActionResult as BadRequestObjectResult;
			Assert.Multiple(() =>
			{
				Assert.That(result?.StatusCode, Is.EqualTo(400));
				Assert.That(result?.Value, Is.EqualTo("Caching failed"));
			});

		}

		[Test]
		public async Task Create_400_Fail_EmptyParameter()
		{
			cacheProcessorMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>(), default)).Returns(Task.FromResult(false));
			var iActionResult = await cacheController.Create("", "string").ConfigureAwait(false);
			var result = iActionResult as BadRequestObjectResult;
			Assert.That(result?.StatusCode, Is.EqualTo(400));
			Assert.That(result?.Value, Is.EqualTo("Key is null or empty"));
		}

		[Test]
		public async Task Create_400_Fail_NullParameter()
		{
			cacheProcessorMock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>(), default)).Returns(Task.FromResult(false));
			var iActionResult = await cacheController.Create("1", null).ConfigureAwait(false);
			var result = iActionResult as BadRequestObjectResult;
			Assert.Multiple(() =>
			{
				Assert.That(result?.StatusCode, Is.EqualTo(400));
				Assert.That(result?.Value, Is.EqualTo("Value is null or empty"));
			});
		}

		[Test]
		public async Task Fetch_200_Success()
		{
			var cacheItem = TestData.CacheItem1();
			cacheProcessorMock.Setup(m => m.Fetch<object>(It.IsAny<string>(), default)).Returns(Task.FromResult(cacheItem.Value));
			var iActionResult = await cacheController.Fetch("1").ConfigureAwait(false);
			var okObjectResult = iActionResult as OkObjectResult;
			Assert.Multiple(() =>
			{
				Assert.That(okObjectResult?.StatusCode, Is.EqualTo(200));
				Assert.That(okObjectResult?.Value, Is.EqualTo("string"));
			});
		}

		[Test]
		public async Task Fetch_400_Fail()
		{
			cacheProcessorMock.Setup(m => m.Fetch<object>(It.IsAny<string>(), default)).Throws<InvalidOperationException>();
			var iActionResult = await cacheController.Fetch("1").ConfigureAwait(false);
			var result = iActionResult as BadRequestObjectResult;
			Assert.That(result?.StatusCode, Is.EqualTo(400));
		}

		[Test]
		public async Task Fetch_400_Fail_EmptyParameter()
		{
			var iActionResult = await cacheController.Fetch("").ConfigureAwait(false);
			var result = iActionResult as BadRequestObjectResult;
			Assert.Multiple(() =>
			{
				Assert.That(result?.StatusCode, Is.EqualTo(400));
				Assert.That(result?.Value, Is.EqualTo("Key is null or empty"));
			});
		}
	}
}