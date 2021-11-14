using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
using Ertis.Scraper.Interactions;
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
				target.Schema = DeserializeSchema(json);
				target.Interactions = DeserializeInteractions(json);
				var crawler = new Crawler(target);
				return await crawler.InitializeAsync();
			}
			else
			{
				throw new Exception("Scraping target schema could not read!");
			}
		}

		private static IEnumerable<FieldInfo> DeserializeSchema(string json)
		{
			var payload = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			if (payload is JObject root)
			{
				return DeserializeSchema(root["schema"] as JObject);
			}
			else
			{
				throw new Exception("Crawler schema is not valid!");
			}
		}
		
		private static IEnumerable<FieldInfo> DeserializeSchema(JObject schemaRoot)
		{
			if (schemaRoot == null)
			{
				yield break;
			}
			
			foreach (var (fieldName, fieldInfoToken) in schemaRoot)
			{
				yield return DeserializeFieldInfo(fieldName, fieldInfoToken);
			}
		}

		private static FieldInfo DeserializeFieldInfo(string fieldName, JToken fieldInfoToken)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new Exception("Field name is required!");
			}
			
			if (fieldInfoToken is JObject fieldInfoJObject)
			{
				return DeserializeFieldInfo(fieldName, fieldInfoJObject);
			}
			else
			{
				throw new Exception("Crawler schema is not valid! Each field item is must be a json object.");
			}
		}

		private static FieldInfo DeserializeFieldInfo(string fieldName, JObject fieldInfoJObject)
		{
			FieldType fieldType;
			string[] selectors = null;
			string attributeName = null;
			string fieldDescription = null;
			FieldOptions fieldOptions = default;
			FieldInfo arrayEnumerator = default;
			FieldInfo[] objectSchema = default;
			
			var dictionary = fieldInfoJObject
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
						objectSchema = DeserializeSchema(dictionary["schema"] as JObject)?.ToArray();
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
				var enumeratorJToken = dictionary["enumerator"] as JObject;
				arrayEnumerator = DeserializeFieldInfo("enumerator", enumeratorJToken);
			}
			
			return new FieldInfo
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

		private static IEnumerable<IInteractionFunction> DeserializeInteractions(string json)
		{
			var payload = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			if (payload is JObject root)
			{
				return DeserializeInteraction(root["interactions"] as JObject);
			}
			else
			{
				throw new Exception("Interactions node is not valid!");
			}
		}
		
		private static IEnumerable<IInteractionFunction> DeserializeInteraction(JObject schemaRoot)
		{
			if (schemaRoot == null)
			{
				yield break;
			}
			
			foreach (var (functionName, interactionToken) in schemaRoot)
			{
				if (interactionToken is JObject interactionJObject)
				{
					if (FunctionFactory.TryCreateFunction(functionName, out var function))
					{
						if (function.Parameters != null)
						{
							foreach (var functionParameter in function.Parameters)
							{
								if (interactionJObject.ContainsKey(functionParameter.Name))
								{
									var stringValue = interactionJObject[functionParameter.Name]?.Value<object>()?.ToString();
									if (functionParameter.Type.TryParse(stringValue, out var value))
									{
										functionParameter.SetValue(value);	
									}
									else
									{
										throw new Exception($"The function parameter '{functionParameter.Name}' could not be parsed to '{functionParameter.Type}' on '{functionName}' function.");
									}
								}
							}
						}
						
						yield return function as IInteractionFunction;
					}	
				}
				else
				{
					throw new Exception("Interaction function node is not valid!");
				}
			}
		}
		
		#endregion
	}
}