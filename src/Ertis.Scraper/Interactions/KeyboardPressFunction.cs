using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Ertis.Scraper.Interactions
{
	public class KeyboardPressFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "keypress";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "key"
					},
					new FunctionParameter<int?>
					{
						Name = "delay"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var key = this.GetParameterValue<string>("key");
			var delay = this.GetParameterValue<int?>("delay");
			var pressOptions = delay != null ? new PressOptions { Delay = delay.Value } : null;
			await page.Keyboard.PressAsync(key, pressOptions);
		}

		#endregion
	}
}