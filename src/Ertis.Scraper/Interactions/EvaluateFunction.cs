using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class EvaluateFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "evaluate";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "script"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var script = this.GetParameterValue<string>("script");
			await page.EvaluateExpressionAsync(script);
		}

		#endregion
	}
}