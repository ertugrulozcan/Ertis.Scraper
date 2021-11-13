using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class AuthenticateFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "authenticate";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "username"
					},
					new FunctionParameter<string>
					{
						Name = "password"
					}
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			var username = this.GetParameterValue<string>("username");
			var password = this.GetParameterValue<string>("password");
			
			await page.AuthenticateAsync(new Credentials { Username = username, Password = password });
		}

		#endregion
	}
}