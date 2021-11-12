using Newtonsoft.Json;

namespace Ertis.Scraper
{
	public class FieldEnumeratorInfo : FieldInfo
	{
		#region Properties
		
		[JsonProperty("item-container")]
		public string ItemContainer { get; set; }
		
		#endregion

		#region Methods

		public static FieldEnumeratorInfo FromBase(FieldInfo fieldInfo, string itemContainer)
		{
			return new FieldEnumeratorInfo
			{
				Name = fieldInfo.Name,
				Description = fieldInfo.Description,
				Type = fieldInfo.Type,
				Route = fieldInfo.Route,
				AttributeName = fieldInfo.AttributeName,
				Options = fieldInfo.Options,
				Enumerator = fieldInfo.Enumerator,
				ItemContainer = itemContainer,
				ObjectSchema = fieldInfo.ObjectSchema
			};
		}

		#endregion
	}
}