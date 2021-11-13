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
			await page.TapAsync(selector);
		}

		#endregion
	}
}