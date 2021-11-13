using System.Threading.Tasks;
using Ertis.Scraper.Extensions;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public class DeleteCookieFunction : FunctionBase, IInteractionFunction
	{
		#region Properties

		public override string Name => "delete_cookie";

		protected override FunctionParameter[] DefaultParameters
		{
			get
			{
				return new FunctionParameter[]
				{
					new FunctionParameter<string>
					{
						Name = "domain"
					},
					new FunctionParameter<int?>
					{
						Name = "expires"
					},
					new FunctionParameter<string>
					{
						Name = "name"
					},
					new FunctionParameter<string>
					{
						Name = "path"
					},
					new FunctionParameter<bool?>
					{
						Name = "secure"
					},
					new FunctionParameter<bool?>
					{
						Name = "session"
					},
					new FunctionParameter<int?>
					{
						Name = "size"
					},
					new FunctionParameter<string>
					{
						Name = "url"
					},
					new FunctionParameter<string>
					{
						Name = "value"
					},
					new FunctionParameter<bool?>
					{
						Name = "httpOnly"
					},
					new FunctionParameter<CookiePolicy?>
					{
						Name = "sameSite"
					},
				};
			}
		}

		#endregion

		#region Methods

		public async Task ExecuteAsync(Page page)
		{
			await page.DeleteCookieAsync(this.GetCookieParam());
		}

		#endregion
	}
}