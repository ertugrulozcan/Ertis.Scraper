using System;
using System.Linq;
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
			
			var frame = this.GetParameterValue<string>("frame");
			if (string.IsNullOrEmpty(frame))
			{
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
							await element.FocusAsync();
						}
						else
						{
							throw new Exception($"Node not found with '{selector}' selector on focus function");
						}
					}
					else
					{
						await currentFrame.FocusAsync(selector);	
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