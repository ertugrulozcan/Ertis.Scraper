using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class ChildrenSelector : HtmlSelectorBase
	{
		#region Properties

		public override bool AllowTraverse => false;

		public override string Token => ">";

		#endregion
		
		#region Methods

		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			return nodes.SelectMany(i => i.ChildNodes);
		}

		#endregion
	}
}