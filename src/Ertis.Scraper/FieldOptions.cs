using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public struct FieldOptions
	{
		#region Properties

		[JsonProperty("format")]
		public FieldFormatting Format { get; set; }

		#endregion
	}
}