using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class GoToFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "goto";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "url"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var url = this.GetParameterValue<string>("url");
			await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
		}

		#endregion
	}
}