using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Scraper.Abstractions;
using Ertis.Scraper.Extensions;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace Ertis.Scraper
{
	public class Crawler : ICrawler
	{
		#region Properties

		private IBrowserContext BrowserContext { get; set; }

		public CrawlerTarget Target { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="target"></param>
		internal Crawler(CrawlerTarget target)
		{
			this.Target = target;
		}

		#endregion

		#region Public Methods

		internal async Task<Crawler> InitializeAsync()
		{
			this.BrowserContext = await Ertis.Scraper.BrowserContext.CreateAsync();
			return this;
		}

		public async Task<ScrapingResult<T>> ScrapeAsync<T>(string url) where T : class
		{
			var result = await this.ScrapeAsync(url);
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.Data);
			var data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
			return new ScrapingResult<T>
			{
				Data = data,
				Errors = result.Errors,
				ElapsedMilliseconds = result.ElapsedMilliseconds
			};
		}

		public async Task<ScrapingResult> ScrapeAsync(string url)
		{
			var stopwatch = Stopwatch.StartNew();
			var htmlDocument = new HtmlDocument();

			try
			{
				var html = await this.FetchHtmlAsync(url, this.Target.Options?.WaitFor);
				htmlDocument.LoadHtml(html);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				return new ScrapingResult
				{
					Data = null,
					Errors = new[] { ex.Message },
					ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
				};
			}

			var result = GetObject(htmlDocument.DocumentNode, this.Target.Schema, out string[] errors);
			stopwatch.Stop();
			
			return new ScrapingResult
			{
				Data = result,
				Errors = errors,
				ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
			};
		}

		#endregion

		#region Private Methods

		private static dynamic GetObject(HtmlNode rootNode, IEnumerable<FieldInfo> schema, out string[] errors)
		{
			var dictionary = new Dictionary<string, object>();
			var errorList = new List<string>();

			foreach (var fieldInfo in schema)
			{
				var value = GetFieldValue(rootNode, fieldInfo, out var innerErrors);
				if (value != null)
				{
					dictionary.Add(fieldInfo.Name, value);
				}

				if (innerErrors != null)
				{
					errorList.AddRange(innerErrors);
				}
			}

			errors = errorList.Any() ? errorList.ToArray() : null;
			return dictionary.ConvertToDynamicObject();
		}

		private static object GetFieldValue(HtmlNode rootNode, FieldInfo fieldInfo, out string[] errors)
		{
			errors = null;
			if (rootNode == null || fieldInfo == null)
			{
				return null;
			}

			try
			{
				var formatter = new FieldFormatter(fieldInfo.Options.Format);
				if (fieldInfo.Type.IsArray)
				{
					var arrayValues = new List<object>();
					var parentNode = rootNode.FindNode(fieldInfo.Route);
					var nodes = parentNode.FindNodes(fieldInfo.Enumerator.ItemContainer);
					var errorList = new List<string>();
					foreach (var node in nodes)
					{
						arrayValues.Add(GetFieldValue(node, fieldInfo.Enumerator, out var innerErrors));
						if (innerErrors != null)
						{
							errorList.AddRange(innerErrors);
						}
					}

					errors = errorList.Any() ? errorList.ToArray() : null;
					return arrayValues;
				}

				if (fieldInfo.Type.IsObject)
				{
					if (fieldInfo.ObjectSchema == null || fieldInfo.ObjectSchema.Length == 0)
					{
						throw new Exception($"Field type declared as object for '{fieldInfo.Name}' but schema missing!");
					}

					return GetObject(rootNode, fieldInfo.ObjectSchema, out errors);
				}
				else
				{
					var node = rootNode.FindNode(fieldInfo.Route);
					if (!string.IsNullOrEmpty(fieldInfo.AttributeName))
					{
						return node.GetAttribute(fieldInfo.AttributeName, fieldInfo.Type.BaseType, formatter);
					}
					else
					{
						return node.GetInnerText(fieldInfo.Type.BaseType, formatter);
					}
				}
			}
			catch (Exception ex)
			{
				errors = new[] { ex.Message };
				return null;
			}
		}

		private async Task<string> FetchHtmlAsync(string url, WaitForOptions waitForOptions = null)
		{
			try
			{
				await using var page = await this.BrowserContext.Browser.NewPageAsync();
				await this.FixUserAgent(page);
				await page.GoToAsync(url);

				if (this.Target.Interactions != null && this.Target.Interactions.Any())
				{
					foreach (var interactionFunction in this.Target.Interactions)
					{
						await interactionFunction.ExecuteAsync(page);
					}
				}
				
				if (waitForOptions != null)
				{
					await page.WaitForSelectorAsync(waitForOptions.Selector, new WaitForSelectorOptions
					{
						Hidden = waitForOptions.Hidden,
						Visible = waitForOptions.Visible,
						Timeout = waitForOptions.TimeOut
					});
				}
				
				return await page.GetContentAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		private async Task FixUserAgent(Page page)
		{
			var userAgent = await this.BrowserContext.Browser.GetUserAgentAsync();
			await page.SetUserAgentAsync(userAgent.Replace("Headless", string.Empty));
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
				this.BrowserContext?.Dispose();
			}

			this.Target = null;
		}

		protected virtual async ValueTask DisposeAsyncCore()
		{
			await this.BrowserContext.DisposeAsync().ConfigureAwait(false);
			this.Target = null;
		}

		#endregion
	}
}