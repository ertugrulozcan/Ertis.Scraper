using System;
using System.Threading.Tasks;
using Ertis.Scraper.Abstractions;
using PuppeteerSharp;

namespace Ertis.Scraper
{
	public class BrowserContext : IBrowserContext
	{
		#region Properties

		public Browser Browser { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		private BrowserContext()
		{ }

		#endregion

		#region Methods

		public static async Task<IBrowserContext> CreateAsync()
		{
			var browserContext = new BrowserContext();
			await browserContext.InitializeAsync();
			return browserContext;
		}

		private async Task InitializeAsync()
		{
			try
			{
				var browserFetcher = new BrowserFetcher();
				await browserFetcher.DownloadAsync();
				this.Browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		#endregion
		
		#region Disposing

		public void Dispose()
		{
			this.Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();
			this.Dispose(disposing: false);
			
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
			GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Browser?.Dispose();
			}

			this.Browser = null;
		}

		protected virtual async ValueTask DisposeAsyncCore()
		{
			await this.Browser.DisposeAsync().ConfigureAwait(false);
			this.Browser = null;
		}

		#endregion
	}
}