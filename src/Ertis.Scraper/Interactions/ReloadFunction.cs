using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class ReloadFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "reload";

		protected override FunctionParameter[] DefaultParameters => null;

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			await page.ReloadAsync();
		}

		#endregion
	}
}