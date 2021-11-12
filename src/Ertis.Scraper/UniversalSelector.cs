using System.Collections.Generic;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class UniversalSelector : HtmlSelectorBase
	{
		#region Properties

		public override string Token => "*";

		#endregion
		
		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			return nodes;
		}

		#endregion
	}
}