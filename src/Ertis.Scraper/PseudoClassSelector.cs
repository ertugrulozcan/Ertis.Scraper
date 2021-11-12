using System.Collections.Generic;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	internal class PseudoClassSelector : HtmlSelectorBase
	{
		#region Properties

		public override string Token => ":";

		#endregion
		
		#region Methods
		
		protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes)
		{
			string[] values = this.Selector.TrimEnd(')').Split(new[] { '(' }, 2);
			var pseudoClass = PseudoClass.GetPseudoClass(values[0]);
			string value = values.Length > 1 ? values[1] : null;
			return pseudoClass.Filter(nodes, value);
		}

		#endregion
	}
}