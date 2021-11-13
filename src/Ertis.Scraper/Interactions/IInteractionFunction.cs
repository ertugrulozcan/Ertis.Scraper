using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public interface IInteractionFunction
	{
		#region Methods

		Task ExecuteAsync(Page page);

		#endregion
	}
}