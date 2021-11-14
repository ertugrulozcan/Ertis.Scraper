using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class KeyboardUpFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "keyup";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "key"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var key = this.GetParameterValue<string>("key");
			await page.Keyboard.UpAsync(key);
		}

		#endregion
	}
}