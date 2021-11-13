using System;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class TapFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "tap";

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
				var xpathQueryResult = await page.XPathAsync(selector);
				if (xpathQueryResult is { Length: > 0 })
				{
					var element = xpathQueryResult[0];
					await element.TapAsync();
				}
				else
				{
					throw new Exception($"Node not found with '{selector}' selector on tap function");
				}
			}
			else
			{
				await page.TapAsync(selector);	
			}
		}

		#endregion
	}
}