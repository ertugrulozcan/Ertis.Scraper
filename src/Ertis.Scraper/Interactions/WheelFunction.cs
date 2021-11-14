using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class WheelFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "wheel";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<int>
					{
						Name = "x"
					},
					new FunctionParameter<int>
					{
						Name = "y"
					}
				};
			}
		}

		#endregion
		
		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var deltaX = this.GetParameterValue<int>("x");
			var deltaY = this.GetParameterValue<int>("y");
			await page.Mouse.WheelAsync(deltaX, deltaY);
		}

		#endregion
	}
}