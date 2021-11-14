using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public struct FieldFormatting
	{
		#region Properties

		[JsonProperty("culture")]
		public string Culture { get; set; }
		
		[JsonProperty("trimStart")]
		public string TrimStart { get; set; }
		
		[JsonProperty("trimEnd")]
		public string TrimEnd { get; set; }
		
		[JsonProperty("appendStart")]
		public string AppendStart { get; set; }
		
		[JsonProperty("appendEnd")]
		public string AppendEnd { get; set; }

		#endregion
	}
}