using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class WaitForOptions
	{
		#region Properties

		[JsonProperty("selector")]
		public string Selector { get; set; }
		
		/// <summary>
		/// Wait for element to not be found in the DOM or to be hidden.
		/// </summary>
		[JsonProperty("hidden")]
		public bool Hidden { get; set; }
		
		/// <summary>
		/// Wait for element to be present in DOM and to be visible.
		/// </summary>
		[JsonProperty("visible")]
		public bool Visible { get; set; }
		
		/// <summary>
		/// Maximum time to wait for in milliseconds. Defaults to `30000` (30 seconds).
		/// Pass `0` to disable timeout.
		/// </summary>
		[JsonProperty("timeout")]
		public int? TimeOut { get; set; }

		#endregion
	}
}