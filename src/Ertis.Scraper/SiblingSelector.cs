using System.Collections.Generic;
using System.Linq;
using Ertis.Scraper.Extensions;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class SiblingSelector : HtmlSelectorBase
	{
		#region Properties

		public override bool AllowTraverse => false;

		public override string Token => "~";

		#endregion
		
		#region Methods
		
		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			foreach (var pivotNode in nodes)
			{
				var idx = pivotNode.GetSelfIndex();
				var children = pivotNode.ParentNode.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element).Skip(idx + 1);
				foreach (var node in children)
				{
					yield return node;
				}
			}
		}

		#endregion
	}
}