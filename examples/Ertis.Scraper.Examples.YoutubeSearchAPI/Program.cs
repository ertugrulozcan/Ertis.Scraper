using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI
{
    public class Program
    {
		#region Methods

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		#endregion
	}
}
