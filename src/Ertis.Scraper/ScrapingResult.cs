using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class ScrapingResult : ScrapingResult<dynamic>
	{ }
	
	public class ScrapingResult<T> where T : class
	{
		#region Properties

		[JsonProperty("data")]
		public T Data { get; internal init; }
		
		[JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] Errors { get; internal init; }

		[JsonProperty("elapsed")]
		public long ElapsedMilliseconds { get; internal init; }
		
		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		internal ScrapingResult()
		{ }

		#endregion
	}
}