using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class CrawlerOptions
	{
		#region Properties

		[JsonProperty("waitfor")]
		public WaitForOptions WaitFor { get; set; }

		#endregion
	}
}