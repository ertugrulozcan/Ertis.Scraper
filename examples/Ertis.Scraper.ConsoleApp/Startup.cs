using System;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Abstractions;
using Ertis.Scraper.Extensions.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Ertis.Scraper.ConsoleApp
{
	public class Startup
	{
		#region Properties

		public IConfiguration Configuration { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		#endregion

		#region Methods

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ITargetProvider, ConfigurationTargetProvider>();
		}

		public void Configure(IHostEnvironment env, IServiceProvider serviceProvider)
		{
			var targetProvider = serviceProvider.GetRequiredService<ITargetProvider>();
			var targets = targetProvider.GetTargets();
			
			RunTestAsync(targets.First()).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		private static async Task RunTestAsync(CrawlerTarget target)
		{
			var crawler = await CrawlerFactory.CreateAsync(target);
			var result = await crawler.ScrapeAsync("<url>");
			var json = JsonConvert.SerializeObject(result, Formatting.Indented);
			Console.WriteLine(json);
		}

		#endregion
	}
}