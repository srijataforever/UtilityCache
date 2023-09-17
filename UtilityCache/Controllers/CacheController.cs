using Microsoft.AspNetCore.Mvc;
using UtilityCache.Interfaces;

namespace UtilityCache.Controllers
{
	/// <summary>
	/// Defines the <see cref="CacheController" />
	/// </summary>
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class CacheController : ControllerBase
	{
		private readonly ICacheProcessor cacheProcessor;

		public CacheController(ICacheProcessor cacheProcessor)
		{
			this.cacheProcessor = cacheProcessor;
		}

		/// <summary>
		/// Set Cache
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesDefaultResponseType]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(string key, [FromBody] object value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return BadRequest("Key is null or empty");
			}

			if (ReferenceEquals(null, value) || ReferenceEquals("", value))
			{
				return BadRequest("Value is null or empty");
			}

			try
			{
				var result = await cacheProcessor.Create(key, value).ConfigureAwait(false);
				if (!result)
				{
					return BadRequest("Caching failed");
				}

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Get Cache
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesDefaultResponseType]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Fetch(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return BadRequest("Key is null or empty");
			}

			try
			{
				var result = await cacheProcessor.Fetch<object>(key).ConfigureAwait(false);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}