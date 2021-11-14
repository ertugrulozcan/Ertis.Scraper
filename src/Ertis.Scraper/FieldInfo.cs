using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class FieldInfo
	{
		#region Properties

		[JsonProperty("name")]
		public string Name { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("type")]
		public FieldType Type { get; set; }
		
		[JsonProperty("route")]
		public string[] Route { get; set; }

		[JsonProperty("attribute")]
		public string AttributeName { get; set; }
		
		[JsonProperty("options")]
		public FieldOptions Options { get; set; }
		
		[JsonProperty("enumerator")]
		public FieldInfo Enumerator { get; set; }
		
		[JsonProperty("schema")]
		public FieldInfo[] ObjectSchema { get; set; }

		#endregion
	}
}