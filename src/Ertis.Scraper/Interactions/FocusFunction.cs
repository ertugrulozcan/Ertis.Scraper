using System.Threading.Tasks;
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
			await page.FocusAsync(selector);
		}

		#endregion
	}
}