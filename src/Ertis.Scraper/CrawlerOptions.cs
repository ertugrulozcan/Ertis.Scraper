using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class CrawlerOptions
	{
		#region Properties

		[JsonProperty("waitfor")]
		public WaitForOptions WaitFor { get; set; }
		
		[JsonProperty("viewport")]
		public ViewportOptions Viewport { get; set; }

		#endregion
	}
}