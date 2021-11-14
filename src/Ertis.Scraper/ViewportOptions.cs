using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class ViewportOptions
	{
		#region Properties

		[JsonProperty("width")] 
		public int Width { get; set; } = 800;

		[JsonProperty("height")] 
		public int Height { get; set; } = 600;

		[JsonProperty("scaleFactor")] 
		public double ScaleFactor { get; set; } = 1;
		
		[JsonProperty("isMobile")]
		public bool IsMobile { get; set; }
		
		[JsonProperty("isLandscape")]
		public bool IsLandscape { get; set; }
		
		[JsonProperty("hasTouch")]
		public bool HasTouch { get; set; }

		#endregion
	}
}