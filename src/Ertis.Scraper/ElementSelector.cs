using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class ElementSelector : HtmlSelectorBase
	{
		#region Properties

		public override string Token => string.Empty;

		#endregion

		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			foreach (var node in nodes)
			{
				if (node.Name.Equals(this.Selector, StringComparison.InvariantCultureIgnoreCase))
				{
					yield return node;
				}
			}
		}

		#endregion
	}
}