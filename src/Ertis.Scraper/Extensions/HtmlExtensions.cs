using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;

namespace Ertis.Scraper.Extensions
{
	public static class HtmlExtensions
	{
		#region Methods

		public static IList<string> GetClassList(this HtmlNode node)
		{
			var attribute = node.Attributes["class"];
			if (attribute == null)
			{
				return Array.Empty<string>();
			}
			
			return attribute.Value.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		}
		
		public static int GetSelfIndex(this HtmlNode node)
		{
			int index = 0;
			var childElements = node.ParentNode.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element);
			foreach (var childNode in childElements)
			{
				if (childNode == node)
				{
					return index;
				}
				
				index++;
			}

			throw new InvalidOperationException("Node not found in its parent!");
		}
		
		public static HtmlNode QuerySelector(this HtmlDocument doc, string cssSelector)
        {
            return doc.QuerySelectorAll(cssSelector).FirstOrDefault();
        }

        public static HtmlNode QuerySelector(this HtmlNode node, string cssSelector)
        {
            return node.QuerySelectorAll(cssSelector).FirstOrDefault();
        }

        public static IList<HtmlNode> QuerySelectorAll(this HtmlDocument doc, string cssSelector)
        {
            return doc.DocumentNode.QuerySelectorAll(cssSelector);
        }

        public static IList<HtmlNode> QuerySelectorAll(this HtmlNode node, string cssSelector)
        {
            return new[] { node }.QuerySelectorAll(cssSelector);
        }
		
        public static IList<HtmlNode> QuerySelectorAll(this IEnumerable<HtmlNode> nodes, string cssSelector)
        {
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));

            if (cssSelector.Contains(','))
			{
				var nodeArray = nodes.ToArray();
                var combinedSelectors = cssSelector.Split(',');
                var founds = nodeArray.QuerySelectorAll(combinedSelectors[0]);
				foreach (var s in combinedSelectors.Skip(1))
				{
					foreach (var node in nodeArray.QuerySelectorAll(s))
					{
						if (!founds.Contains(node))
						{
							founds.Add(node);
						}
					}
				}

                return founds;
            }

            cssSelector = cssSelector.Trim();
			var selectors = HtmlSelectorBase.Parse(cssSelector);
			bool allowTraverse = true;
			foreach (var selector in selectors)
            {
				if (allowTraverse && selector.AllowTraverse)
				{
					nodes = Traverse(nodes);
				}

                nodes = selector.Filter(nodes);
                allowTraverse = selector.AllowTraverse;
            }

            return nodes.Distinct().ToList();
        }
		
        private static IEnumerable<HtmlNode> Traverse(IEnumerable<HtmlNode> nodes)
        {
			foreach (var pivotNode in nodes)
			{
				foreach (var node in Traverse(pivotNode).Where(i => i.NodeType == HtmlNodeType.Element))
				{
					yield return node;
				}
			}
        }
		
        private static IEnumerable<HtmlNode> Traverse(HtmlNode root)
        {
            yield return root;

			foreach (var child in root.ChildNodes)
			{
				foreach (var node in Traverse(child))
				{
					yield return node;
				}
			}
        }

		public static HtmlNode FindNode(this HtmlNode root, IEnumerable<string> route)
		{
			var pivotNode = root;
			var parentSelector = string.Empty;
			foreach (var selector in route)
			{
				var parentNode = pivotNode;
				pivotNode = pivotNode.QuerySelector(selector);
				if (pivotNode == null)
				{
					var parentElementToken = parentNode.Name;
					if (!string.IsNullOrEmpty(parentSelector))
					{
						parentElementToken += $"[{parentSelector}]";
					}
					
					throw new Exception($"Node not found with '{selector}' selector in {parentElementToken}");
				}
				
				parentSelector = selector;
			}

			return pivotNode;
		}

		public static IEnumerable<HtmlNode> FindNodes(this HtmlNode root, string selector)
		{
			return root?.QuerySelectorAll(selector);
		}
		
		public static object GetAttribute(this HtmlNode node, string attributeName, Type type, FieldFormatter formatter)
		{
			var attribute = node.Attributes.FirstOrDefault(x => x.Name == attributeName);
			if (attribute != null)
			{
				var cultureInfo = string.IsNullOrEmpty(formatter.Options.Culture) ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(formatter.Options.Culture);
				if (type.TryParse(attribute.Value, formatter, cultureInfo, out var value))
				{
					return value;
				}
				else
				{
					throw new Exception($"Attribute value ({attribute.Value}) can not be convert to {type.Name}");
				}
			}
			else
			{
				throw new Exception($"Attribute is not exist with name '{attributeName}' in '{node.Name}' node");
			}
		}
		
		public static object GetInnerText(this HtmlNode node, Type type, FieldFormatter formatter)
		{
			var cultureInfo = string.IsNullOrEmpty(formatter.Options.Culture) ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(formatter.Options.Culture);
			if (type.TryParse(node.InnerText, formatter, cultureInfo, out var value))
			{
				return value;
			}
			else
			{
				throw new Exception($"Inner text can not be convert to {type.Name}");
			}
		}

		#endregion
	}
}