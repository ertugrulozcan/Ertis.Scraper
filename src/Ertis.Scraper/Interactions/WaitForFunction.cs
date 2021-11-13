using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class WaitForFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "waitfor";

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
					new FunctionParameter<bool>
					{
						Name = "hidden"
					},
					new FunctionParameter<bool>
					{
						Name = "visible"
					},
					new FunctionParameter<int?>
					{
						Name = "timeout"
					}
				};
			}
		}

		#endregion
		
		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var selector = this.GetParameterValue<string>("selector");
			var hidden = this.GetParameterValue<bool>("hidden");
			var visible = this.GetParameterValue<bool>("visible");
			var timeout = this.GetParameterValue<int?>("timeout");
			var options = new WaitForSelectorOptions { Hidden = hidden, Visible = visible, Timeout = timeout };
			
			if (selector.StartsWith(XPathSelector.XPathSelectorToken))
			{
				await page.WaitForXPathAsync(selector, options);
			}
			else
			{
				await page.WaitForSelectorAsync(selector, options);
			}
		}

		#endregion
	}
}