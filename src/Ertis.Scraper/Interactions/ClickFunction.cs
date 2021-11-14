using System;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
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
					new FunctionParameter<int?>
					{
						Name = "x"
					},
					new FunctionParameter<int?>
					{
						Name = "y"
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
			var x = this.GetParameterValue<int?>("x");
			var y = this.GetParameterValue<int?>("y");
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

			if (!string.IsNullOrEmpty(selector))
			{
				await this.ClickSelectorAsync(page, selector, clickOptions);
			}
			else if (x != null && y != null)
			{
				await this.ClickCoordinateAsync(page, x.Value, y.Value, clickOptions);
			}
			else
			{
				throw new ArgumentNullException(nameof(selector), "Selector or coordinate parameters required for click function!");
			}
		}

		private async Task ClickSelectorAsync(Page page, string selector, ClickOptions clickOptions)
		{
			var frame = this.GetParameterValue<string>("frame");
			if (string.IsNullOrEmpty(frame))
			{
				if (selector.StartsWith(XPathSelector.XPathSelectorToken))
				{
					var element = await page.QuerySelectorByXPath(selector);
					if (element != null)
					{
						await element.ClickAsync();
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
			else
			{
				var currentFrame = page.Frames.FirstOrDefault(x => x.Name == frame);
				if (currentFrame != null)
				{
					if (selector.StartsWith(XPathSelector.XPathSelectorToken))
					{
						var element = await currentFrame.QuerySelectorByXPath(selector);
						if (element != null)
						{
							await element.ClickAsync();
						}
						else
						{
							throw new Exception($"Node not found with '{selector}' selector on click function");
						}
					}
					else
					{
						await currentFrame.ClickAsync(selector, clickOptions);	
					}
				}
				else
				{
					throw new Exception($"Frame not found with name '{frame}'");
				}
			}
		}
		
		private async Task ClickCoordinateAsync(Page page, int x, int y, ClickOptions clickOptions)
		{
			await page.Mouse.ClickAsync(x, y, clickOptions);
		}

		#endregion
	}
}