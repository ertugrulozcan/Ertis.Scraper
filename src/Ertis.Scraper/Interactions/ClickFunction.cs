using System;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Ertis.Scraper.Interactions
{
	public class ClickFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "click";

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
					new FunctionParameter<MouseButtonType?>
					{
						Name = "button"
					},
					new FunctionParameter<int?>
					{
						Name = "delay"
					},
					new FunctionParameter<int?>
					{
						Name = "clickCount"
					}
				};
			}
		}

		#endregion
		
		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var selector = this.GetParameterValue<string>("selector");
			var button = this.GetParameterValue<MouseButtonType?>("button");
			var delay = this.GetParameterValue<int?>("delay");
			var clickCount = this.GetParameterValue<int?>("clickCount");
			ClickOptions clickOptions = null;
			if (button != null || delay != null || clickCount != null)
			{
				clickOptions = new ClickOptions();
				if (button != null)
				{
					clickOptions.Button = button.Value switch
					{
						MouseButtonType.Left => MouseButton.Left,
						MouseButtonType.Right => MouseButton.Right,
						MouseButtonType.Middle => MouseButton.Middle,
						MouseButtonType.None => MouseButton.None,
						_ => throw new ArgumentOutOfRangeException()
					};
				}

				if (delay != null)
				{
					clickOptions.Delay = delay.Value;
				}

				if (clickCount != null)
				{
					clickOptions.ClickCount = clickCount.Value;
				}
			}

			if (selector.StartsWith(XPathSelector.XPathSelectorToken))
			{
				var xpathQueryResult = await page.XPathAsync(selector);
				if (xpathQueryResult is { Length: > 0 })
				{
					var element = xpathQueryResult[0];
					await element.ClickAsync(clickOptions);
				}
				else
				{
					throw new Exception($"Node not found with '{selector}' selector on click function");
				}
			}
			else
			{
				await page.ClickAsync(selector, clickOptions);	
			}
		}

		#endregion
	}
}