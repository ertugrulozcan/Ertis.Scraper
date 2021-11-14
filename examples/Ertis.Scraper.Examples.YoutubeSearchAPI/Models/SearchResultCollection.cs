using Newtonsoft.Json;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Models
{
	public class SearchResultCollection
	{
		#region Properties

		[JsonProperty("items")]
		public SearchResultItem[] Items { get; set; }

		#endregion
	}
}