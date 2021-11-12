using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class IdSelector : HtmlSelectorBase
	{
		#region Properties

		public override string Token => "#";

		#endregion
		
		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			foreach (var node in nodes)
			{
				if (node.Id.Equals(this.Selector, StringComparison.InvariantCultureIgnoreCase))
				{
					return new[] { node };
				}
			}

			return Array.Empty<HtmlNode>();
		}

		#endregion
	}
}