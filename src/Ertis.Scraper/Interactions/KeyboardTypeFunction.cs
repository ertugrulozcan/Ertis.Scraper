using System;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
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
			
			var frame = this.GetParameterValue<string>("frame");
			if (string.IsNullOrEmpty(frame))
			{
				if (selector.StartsWith(XPathSelector.XPathSelectorToken))
				{
					var element = await page.QuerySelectorByXPath(selector);
					if (element != null)
					{
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
							await element.TypeAsync(text, typeOptions);
						}
						else
						{
							throw new Exception($"Node not found with '{selector}' selector on type function");
						}
					}
					else
					{
						await currentFrame.TypeAsync(selector, text, typeOptions);	
					}
				}
				else
				{
					throw new Exception($"Frame not found with name '{frame}'");
				}
			}
		}

		#endregion
	}
}