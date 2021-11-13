using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class GoForwardFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "goforward";

		protected override FunctionParameter[] DefaultParameters => null;

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			await page.GoForwardAsync();
		}

		#endregion
	}
}