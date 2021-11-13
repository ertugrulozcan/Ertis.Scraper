using System;
using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class FocusFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "focus";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "selector"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var selector = this.GetParameterValue<string>("selector");
			if (selector.StartsWith(XPathSelector.XPathSelectorToken))
			{
				var element = await page.QuerySelectorByXPath(selector);
				if (element != null)
				{
					await element.FocusAsync();
				}
				else
				{
					throw new Exception($"Node not found with '{selector}' selector on focus function");
				}
			}
			else
			{
				await page.FocusAsync(selector);	
			}
		}

		#endregion
	}
}