using System.Threading.Tasks;
using Ertis.Scraper.Examples.YoutubeSearchAPI.Models;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI.Services
{
	public interface ISearchService
	{
		Task<SearchResult> SearchAsync(string key);
	}
}