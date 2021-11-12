using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ertis.Scraper.ConsoleApp
{
	internal class Program
	{
		#region Main

		public static void Main(string[] args)
		{
			var configurationBuilder = new ConfigurationBuilder();
			var configuration = BuildConfiguration(configurationBuilder);

			IHostEnvironment hostingEnvironment = null;
			var startup = new Startup(configuration);
			var hostBuilder = Host.CreateDefaultBuilder();

			hostBuilder.ConfigureLogging((context, loggingBuilder) =>
			{
				loggingBuilder.Services.AddLogging();
				Enum.TryParse(context.Configuration["Logging:LogLevel:Default"], out LogLevel defaultLogLevel);
				loggingBuilder.AddConsole(options => options.LogToStandardErrorThreshold = defaultLogLevel);
			});

			var host = hostBuilder.ConfigureServices((context, services) =>
				{
					hostingEnvironment = context.HostingEnvironment;
					startup.ConfigureServices(services);
				})
				.Build();

			startup.Configure(hostingEnvironment, host.Services);
		}

		#endregion

		#region Configuration

		private static IConfiguration BuildConfiguration(IConfigurationBuilder configurationBuilder)
		{
			configurationBuilder = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			if (!string.IsNullOrEmpty(environment))
			{
				configurationBuilder = configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
			}

			configurationBuilder = configurationBuilder.AddEnvironmentVariables();

			return configurationBuilder.Build();
		}

		#endregion
	}
}