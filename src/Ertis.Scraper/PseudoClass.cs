using System;
using System.Collections.Generic;
using System.Linq;
using Ertis.Scraper.Annotations;
using Ertis.Scraper.Extensions;
using HtmlAgilityPack;

namespace Ertis.Scraper
{
	public abstract class PseudoClass
	{
		#region Fields

		private static readonly Dictionary<string, PseudoClass> pseudoClassInstances = CreatePseudoClassInstances();

		#endregion

		#region Properties



		#endregion

		#region Abstract Methods

		protected abstract bool CheckNode(HtmlNode node, string parameter);

		#endregion

		#region Virtual Methods

		public virtual IEnumerable<HtmlNode> Filter(IEnumerable<HtmlNode> nodes, string parameter)
		{
			return nodes.Where(i => CheckNode(i, parameter));
		}

		#endregion

		#region Methods

		public static PseudoClass GetPseudoClass(string pseudoClass)
		{
			if (!pseudoClassInstances.ContainsKey(pseudoClass))
			{
				throw new NotSupportedException("Unsupported pseudo class: " + pseudoClass);
			}

			return pseudoClassInstances[pseudoClass];
		}

		private static Dictionary<string, PseudoClass> CreatePseudoClassInstances()
		{
			var rt = new Dictionary<string, PseudoClass>(StringComparer.InvariantCultureIgnoreCase);
			Func<System.Reflection.Assembly, Type[]> tryGetTypes = a =>
			{
				return a.IsDynamic ? new Type[] { } : a.GetTypes();
			};

			var types = 
				AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(asm => tryGetTypes(asm)
						.Where(i => !i.IsAbstract && i.IsSubclassOf(typeof(PseudoClass))));
			
			types = types.OrderBy(i => i.Assembly == typeof(PseudoClass).Assembly ? 0 : 1).ToList();
			foreach (var type in types)
			{
				var attribute = 
					type.GetCustomAttributes(typeof(PseudoClassNameAttribute), false)
						.Cast<PseudoClassNameAttribute>().FirstOrDefault();
				
				if (attribute != null)
				{
					rt.Add(attribute.FunctionName, (PseudoClass)Activator.CreateInstance(type));	
				}
			}

			return rt;
		}

		#endregion
	}
	
	[PseudoClassName("first-child")]
	internal class FirstChildPseudoClass : PseudoClass
	{
		protected override bool CheckNode(HtmlNode node, string parameter)
		{
			return node.GetSelfIndex() == 0;
		}
	}
	
	[PseudoClassName("last-child")]
	internal class LastChildPseudoClass : PseudoClass
	{
		protected override bool CheckNode(HtmlNode node, string parameter)
		{
			var children = node.ParentNode.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element);
			return children.Last() == node;
		}
	}
	
	[PseudoClassName("not")]
	internal class NotPseudoClass : PseudoClass
	{
		protected override bool CheckNode(HtmlNode node, string parameter)
		{
			var selectors = HtmlSelectorBase.Parse(parameter);
			var nodes = new[] { node };
			foreach (var selector in selectors)
			{
				if (selector.FilterCore(nodes).Count() == 1)
				{
					return false;
				}
			}

			return true;
		}
	}
	
	[PseudoClassName("nth-child")]
	internal class NthChildPseudoClass : PseudoClass
	{
		protected override bool CheckNode(HtmlNode node, string parameter)
		{
			return node.GetSelfIndex() == int.Parse(parameter) - 1;
		}
	}
}