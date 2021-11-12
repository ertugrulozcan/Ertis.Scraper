using System.Collections.Generic;
using System.Dynamic;

namespace Ertis.Scraper.Extensions
{
	public static class DynamicExtensions
	{
		#region Methods

		public static dynamic ConvertToDynamicObject(this IDictionary<string, object> dictionary)
		{
			var expandoObject = new ExpandoObject();
			var expandoObjectProperties = (ICollection<KeyValuePair<string, object>>) expandoObject;
			foreach (var pair in dictionary)
			{
				expandoObjectProperties.Add(pair);
			}

			dynamic dynamicObject = expandoObjectProperties;
			return dynamicObject;
		}

		#endregion
	}
}