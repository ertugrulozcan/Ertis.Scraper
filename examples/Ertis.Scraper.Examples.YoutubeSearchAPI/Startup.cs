using Ertis.Scraper.Abstractions;
using Ertis.Scraper.Examples.YoutubeSearchAPI.Services;
using Ertis.Scraper.Extensions.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Ertis.Scraper.Examples.YoutubeSearchAPI
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
			services.AddSingleton<ISearchService, SearchService>();
			
			services.AddControllers().AddNewtonsoftJson();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ertis.Scraper.Examples.YoutubeSearchAPI", Version = "v1" });
			});
		}
		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ertis.Scraper.Examples.YoutubeSearchAPI v1"));
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		#endregion
    }
}
