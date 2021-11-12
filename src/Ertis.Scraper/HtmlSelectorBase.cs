using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ertis.Scraper
{
	public abstract class HtmlSelectorBase : IHtmlSelector
	{
		#region Fields

		private static readonly HtmlSelectorBase[] selectorInstances = CrateSelectorInstances();

		#endregion
		
		#region Properties
		
		public abstract string Token { get; }
		
		protected virtual bool IsSubSelector => false;

		public virtual bool AllowTraverse => true;

		public IList<HtmlSelectorBase> SubSelectors { get; set; }
		
		public string Selector { get; set; }
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Constructor
		/// </summary>
        protected HtmlSelectorBase()
        {
            this.SubSelectors = new List<HtmlSelectorBase>();
        }
        
		#endregion

		#region Abstract Methods

		protected internal abstract IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> nodes);

		#endregion
		
        #region Methods
		
        public IEnumerable<HtmlNode> Filter(IEnumerable<HtmlNode> nodes)
        {
			nodes = this.FilterCore(nodes).Distinct();
			if (!this.SubSelectors.Any())
                return nodes;

			foreach (var selector in this.SubSelectors)
			{
				nodes = selector.FilterCore(nodes);
			}

            return nodes;
        }

        public virtual string GetSelectorParameter(string selector)
        {
            return selector.Substring(this.Token.Length);
        }

        public static IList<HtmlSelectorBase> Parse(string cssSelector)
        {
            var selectorList = new List<HtmlSelectorBase>();
            var tokens = SelectorTokenizer.GetTokens(cssSelector);
			foreach (var token in tokens)
			{
				selectorList.Add(ParseSelector(token));
			}

            return selectorList;
        }

        private static HtmlSelectorBase ParseSelector(SelectorToken token)
        {
            HtmlSelectorBase currentSelector;
			if (token.Filter.StartsWith(XPathSelector.XPathSelectorToken, StringComparison.InvariantCultureIgnoreCase))
			{
				currentSelector = selectorInstances.FirstOrDefault(x => x.Token == XPathSelector.XPathSelectorToken);
			}
			else if (char.IsLetter(token.Filter[0]))
			{
				currentSelector = selectorInstances.First(i => i is ElementSelector);
			}
			else
			{
				currentSelector = selectorInstances
					.Where(s => s.Token.Length > 0)
					.FirstOrDefault(s => token.Filter.StartsWith(s.Token));
			}

			if (currentSelector == null)
			{
				throw new InvalidOperationException("Invalid token: " + token.Filter);
			}

            var selector = (HtmlSelectorBase)Activator.CreateInstance(currentSelector.GetType());
			if (selector == null)
			{
				throw new InvalidOperationException("Unsupported selector type: " + currentSelector.GetType());
			}
			
			selector.SubSelectors = token.SubTokens.Select(ParseSelector).ToList();
			selector.Selector = token.Filter.Substring(currentSelector.Token.Length);
			
            return selector;
        }

        private static HtmlSelectorBase[] CrateSelectorInstances()
        {
            var assembly = typeof(HtmlSelectorBase).Assembly;
            Func<Type, bool> typeQuery = type => type.IsSubclassOf(typeof(HtmlSelectorBase)) && !type.IsAbstract;

            var defaultTypes = assembly.GetTypes().Where(typeQuery);
            var types = 
				AppDomain.CurrentDomain.GetAssemblies()
					.Where(x => x == assembly)
					.SelectMany(x => x.GetTypes().Where(typeQuery));
			
            types = defaultTypes.Concat(types);
			return types.Select(Activator.CreateInstance).Cast<HtmlSelectorBase>().ToArray();
		}

        #endregion
	}
}