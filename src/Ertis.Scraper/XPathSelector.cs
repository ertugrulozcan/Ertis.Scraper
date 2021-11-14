using System.Collections.Generic;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	public class XPathSelector : HtmlSelectorBase
	{
		#region Constants

		internal const string XPathSelectorToken = "/";

		#endregion
		
		#region Properties

		public override string Token => XPathSelectorToken;

		#endregion

		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			foreach (var node in nodes)
			{
				var matchedNodes = node.SelectNodes($"{XPathSelectorToken}{this.Selector}");
				if (matchedNodes != null)
				{
					foreach (var matchedNode in matchedNodes)
					{
						yield return matchedNode;	
					}	
				}
			}
		}

		#endregion
	}
}