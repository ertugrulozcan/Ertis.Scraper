using System;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Abstractions;
using Ertis.Scraper.Examples.YoutubeSearchAPI.Models;
using Ertis.Scraper.Interactions;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Services
{
	public class SearchService : ISearchService
	{
		#region Services

		private readonly CrawlerTarget _target;
		private readonly Crawler _crawler;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="targetProvider"></param>
		public SearchService(ITargetProvider targetProvider)
		{
			this._target = targetProvider.GetTarget("youtube");
			this._crawler = CrawlerFactory.CreateAsync(this._target).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		#endregion
		
		#region Methods

		public async Task<SearchResult> SearchAsync(string key)
		{
			this.SetSearchQueryParameter(key);
			var result = await this._crawler.ScrapeAsync<SearchResultCollection>($"https://{this._target.Domain}");
			return new SearchResult
			{
				Key = key,
				Result = result
			};
		}

		private void SetSearchQueryParameter(string key)
		{
			if (this._crawler.Target.Interactions.FirstOrDefault(x => x.Name == "type") is KeyboardTypeFunction keyboardTypeFunction)
			{
				var functionParameter = keyboardTypeFunction.Parameters.FirstOrDefault(x => x.Name == "text");
				if (functionParameter != null)
				{
					functionParameter.SetValue(key);	
				}
				else
				{
					throw new Exception("Text parameter missing in keyboard typing function!");
				}
			}
			else
			{
				throw new Exception("Keyboard typing function missing on interaction schema!");
			}
		}

		#endregion
	}
}