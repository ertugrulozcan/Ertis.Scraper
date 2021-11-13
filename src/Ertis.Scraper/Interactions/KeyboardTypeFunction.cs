using System;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Ertis.Scraper.Interactions
{
	public class KeyboardTypeFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "type";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "selector"
					},
					new FunctionParameter<string>
					{
						Name = "text"
					},
					new FunctionParameter<int?>
					{
						Name = "delay"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var selector = this.GetParameterValue<string>("selector");
			var text = this.GetParameterValue<string>("text");
			var delay = this.GetParameterValue<int?>("delay");
			var typeOptions = delay != null ? new TypeOptions { Delay = delay.Value } : null;
			
			if (selector.StartsWith(XPathSelector.XPathSelectorToken))
			{
				var xpathQueryResult = await page.XPathAsync(selector);
				if (xpathQueryResult is { Length: > 0 })
				{
					var element = xpathQueryResult[0];
					await element.TypeAsync(text, typeOptions);
				}
				else
				{
					throw new Exception($"Node not found with '{selector}' selector on type function");
				}
			}
			else
			{
				await page.TypeAsync(selector, text, typeOptions);	
			}
		}

		#endregion
	}
}