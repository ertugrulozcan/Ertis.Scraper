using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class AttributeSelector : HtmlSelectorBase
	{
		#region Properties

		public override string Token => "[";

		#endregion
		
		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			var filter = this.GetFilter();
			foreach (var node in nodes)
			{
				if (filter(node))
				{
					yield return node;
				}
			}
		}

		private Func<HtmlNode, bool> GetFilter()
		{
			string filter = this.Selector.Trim('[', ']');

			int index = filter.IndexOf('=');
			if (index == 0)
				throw new InvalidOperationException("Invalid selector: " + this.Selector);

			if (index < 0)
			{
				return node => node.Attributes.Contains(filter);
			}

			var filterExpression = this.GetFilterExpression(filter[index - 1]);
			if (!char.IsLetterOrDigit(filter[index - 1]))
			{
				filter = filter.Remove(index - 1, 1);
			}

			string[] values = filter.Split(new[] { '=' }, 2);
			filter = values[0];
			string value = values[1];
			if (value[0] == value[value.Length - 1] && (value[0] == '"' || value[0] == '\''))
			{
				value = value.Substring(1, value.Length - 2);
			}

			return node => node.Attributes.Contains(filter) && filterExpression(node.Attributes[filter].Value, value);
		}

		private Func<string, string, bool> GetFilterExpression(char value)
		{
			if (char.IsLetterOrDigit(value))
				return (attr, v) => attr == v;

			return value switch
			{
				'*' => (attr, v) => attr == v || attr.Contains(v),
				'^' => (attr, v) => attr.StartsWith(v),
				'$' => (attr, v) => attr.EndsWith(v),
				'~' => (attr, v) => attr.Split(' ').Contains(v),
				_ => throw new NotSupportedException("Invalid selector: " + this.Selector)
			};
		}

		#endregion
	}
}