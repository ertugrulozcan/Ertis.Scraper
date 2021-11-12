using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class CrawlerTarget
	{
		#region Properties

		[JsonProperty("name")]
		public string Name { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("domain")]
		public string Domain { get; set; }
		
		[JsonIgnore]
		public IEnumerable<FieldInfo> Schema { get; set; }

		[JsonProperty("options")]
		public CrawlerOptions Options { get; set; }
		
		#endregion
	}
}