using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
using Newtonsoft.Json.Linq;

namespace Ertis.Scraper
{
	public static class CrawlerFactory
	{
		#region Methods

		public static async Task<Crawler> CreateAsync(CrawlerTarget target)
		{
			var crawler = new Crawler(target);
			return await crawler.InitializeAsync();
		}
		
		public static async Task<Crawler> CreateFromJsonAsync(string json)
		{
			var target = Newtonsoft.Json.JsonConvert.DeserializeObject<CrawlerTarget>(json);
			if (target != null)
			{
				target.Schema = ReadMappingsFromJson(json);
				var crawler = new Crawler(target);
				return await crawler.InitializeAsync();
			}
			else
			{
				throw new Exception("Scraping target schema could not read!");
			}
		}
		
		private static IEnumerable<FieldInfo> ReadMappingsFromJson(string json)
		{
			var payload = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			if (payload is JObject root)
			{
				foreach (var (fieldName, crawlerToken) in root)
				{
					FieldType fieldType;
					string[] selectors;
					string attributeName = null;
					string fieldDescription = null;
					FieldOptions fieldOptions = default;
					FieldEnumeratorInfo arrayEnumerator = default;
					FieldInfo[] objectSchema = default;

					if (string.IsNullOrEmpty(fieldName))
					{
						throw new Exception("Field name is required!");
					}
					
					if (crawlerToken is JObject crawlerTokenObject)
					{
						var dictionary = crawlerTokenObject
							.GetEnumerator()
							.ToEnumerable()
							.ToDictionary(x => x.Key, y => y.Value);
						
						if (dictionary.ContainsKey("type"))
						{
							if (!(dictionary["type"] is JValue { Value: { } } jValue && FieldType.TryParse(jValue.Value.ToString(), out fieldType)))
							{
								throw new Exception($"Field type could not be serialized for '{fieldName}'");	
							}
							else if (fieldType.IsObject)
							{
								if (dictionary.ContainsKey("schema"))
								{
									objectSchema = ReadMappingsFromJson(dictionary["schema"]?.ToString())?.ToArray();
								}
								else
								{
									throw new Exception($"Field type declared as object for '{fieldName}' but schema missing!");
								}
							}
						}
						else
						{
							throw new Exception($"Field type missing for '{fieldName}'");
						}
						
						if (dictionary.ContainsKey("route"))
						{
							if (dictionary["route"] is not JArray jArray)
							{
								throw new Exception($"Route could not be serialized to string array for '{fieldName}'");	
							}
							else
							{
								selectors = jArray.Select(x => x as JValue).Select(x => x?.Value?.ToString()).ToArray();
							}
						}
						else
						{
							throw new Exception($"Route missing for '{fieldName}'");
						}
						
						if (dictionary.ContainsKey("attribute"))
						{
							if (dictionary["attribute"] is JValue jValue)
							{
								attributeName = jValue.Value?.ToString();
							}
						}
						
						if (dictionary.ContainsKey("description"))
						{
							if (dictionary["description"] is JValue jValue)
							{
								fieldDescription = jValue.Value?.ToString();
							}
						}
						
						if (dictionary.ContainsKey("options") && dictionary["options"] != null)
						{
							fieldOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldOptions>(dictionary["options"].ToString());
						}
						
						if (dictionary.ContainsKey("enumerator") && dictionary["enumerator"] != null)
						{
							arrayEnumerator = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldEnumeratorInfo>(dictionary["enumerator"].ToString());
						}
					}
					else
					{
						throw new Exception("Crawler schema is not valid! Each field item is must be a json object.");
					}

					yield return new FieldInfo
					{
						Name = fieldName,
						Type = fieldType,
						Route = selectors,
						AttributeName = attributeName,
						Description = fieldDescription,
						Options = fieldOptions,
						Enumerator = arrayEnumerator,
						ObjectSchema = objectSchema
					};
				}
			}
			else
			{
				throw new Exception("Crawler schema is not valid!");
			}
		}
		
		#endregion
	}
}