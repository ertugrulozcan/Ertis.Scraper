using System.Threading.Tasks;
using PuppeteerSharp;

namespace Ertis.Scraper.Interactions
{
	public interface IInteractionFunction
	{
		#region Properties
		
		string Name { get; }
		
		string Comment { get; }
		
		#endregion
		
		#region Methods

		Task ExecuteAsync(Page page);

		#endregion
	}
}