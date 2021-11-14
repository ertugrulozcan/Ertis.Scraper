using Newtonsoft.Json;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Models
{
	public class SearchResult
	{
		#region Properties

		[JsonProperty("key")]
		public string Key { get; set; }
		
		[JsonProperty("result")]
		public ScrapingResult<SearchResultCollection> Result { get; set; }

		#endregion
	}
}