using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class WaitFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "wait";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<int>
					{
						Name = "duration"
					}
				};
			}
		}

		#endregion
		
		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var duration = this.GetParameterValue<int>("duration");
			await page.WaitForTimeoutAsync(duration);
		}

		#endregion
	}
}