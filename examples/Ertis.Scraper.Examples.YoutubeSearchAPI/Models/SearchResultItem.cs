using Newtonsoft.Json;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Models
{
	public class SearchResultItem
	{
		#region Properties

		[JsonProperty("title")]
		public string Title { get; set; }
		
		[JsonProperty("url")]
		public string Url { get; set; }
		
		[JsonProperty("thumbnail")]
		public string Thumbnail { get; set; }

		#endregion
	}
}