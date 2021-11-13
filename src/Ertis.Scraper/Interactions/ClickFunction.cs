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
					switch (button.Value)
					{
						case MouseButtonType.Left:
							clickOptions.Button = MouseButton.Left;
							break;
						case MouseButtonType.Right:
							clickOptions.Button = MouseButton.Right;
							break;
						case MouseButtonType.Middle:
							clickOptions.Button = MouseButton.Middle;
							break;
						case MouseButtonType.None:
							clickOptions.Button = MouseButton.None;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
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
			
			await page.ClickAsync(selector, clickOptions);
		}

		#endregion
	}
}