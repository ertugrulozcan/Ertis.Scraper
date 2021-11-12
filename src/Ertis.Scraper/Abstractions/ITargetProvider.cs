using System.Collections.Generic;

namespace Ertis.Scraper.Abstractions
{
	public interface ITargetProvider
	{
		IEnumerable<CrawlerTarget> GetTargets();
		
		CrawlerTarget GetTarget(string name);
	}
}