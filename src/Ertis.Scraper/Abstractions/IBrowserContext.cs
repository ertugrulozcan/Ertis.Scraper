using System;
using PuppeteerSharp;

namespace Ertis.Scraper.Abstractions
{
	public interface IBrowserContext : IDisposable, IAsyncDisposable
	{
		Browser Browser { get; }
	}
}