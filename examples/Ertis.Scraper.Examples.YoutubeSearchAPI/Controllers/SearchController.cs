using System;
using System.Threading.Tasks;
using Ertis.Scraper.Examples.YoutubeSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Controllers
{
    [ApiController]
    [Route("api/v1/search")]
    public class SearchController : ControllerBase
    {
		#region Services

		private readonly ISearchService _searchService;

		#endregion
		
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="searchService"></param>
		public SearchController(ISearchService searchService)
		{
			this._searchService = searchService;
		}

		#endregion
		
		#region Methods

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string key)
		{
			try
			{
				return this.Ok(await this._searchService.SearchAsync(key));
			}
			catch (Exception ex)
			{
				return this.StatusCode(500, ex);
			}
		}

		#endregion
    }
}
