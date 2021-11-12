using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ertis.Scraper.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Ertis.Scraper.Extensions.AspNetCore
{
	public class ConfigurationTargetProvider : ITargetProvider
	{
		#region Services

		private readonly IConfiguration configuration;

		#endregion
		
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		public ConfigurationTargetProvider(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		#endregion
		
		#region Methods

		public IEnumerable<CrawlerTarget> GetTargets()
		{
			const string targetsSectionKey = "Targets";

			var targetsSection = configuration.GetSection(targetsSectionKey);
			var targetSections = targetsSection.GetChildren();
			foreach (var targetSection in targetSections)
			{
				var crawlerTarget = targetSection.Get<CrawlerTarget>();
				var fieldInfos = new List<FieldInfo>();
				var fieldSections = targetSection.GetSection("schema").GetChildren();
				foreach (var fieldSection in fieldSections)
				{
					fieldInfos.Add(ParseToFieldInfo(fieldSection));
				}

				crawlerTarget.Schema = fieldInfos;

				yield return crawlerTarget;
			}
		}
		
		public CrawlerTarget GetTarget(string name)
		{
			return this.GetTargets().FirstOrDefault(x => x.Name == name);
		}
		
		private static FieldInfo ParseToFieldInfo([NotNull] IConfigurationSection fieldSection)
		{
			if (fieldSection == null)
			{
				return null;
			}
			
			var fieldName = fieldSection.Key;
			var fieldDescription = fieldSection.GetValue<string>("description");
			var fieldTypeName = fieldSection.GetValue<string>("type");
			var fieldRoute = fieldSection.GetSection("route").Get<string[]>();
			var fieldAttributeName = fieldSection.GetValue<string>("attribute");
			var fieldOptions = fieldSection.GetSection("options").Get<FieldOptions>();
			var fieldArrayEnumeratorSection = fieldTypeName == "array" ? fieldSection.GetSection("enumerator") : null;

			if (string.IsNullOrEmpty(fieldName))
			{
				throw new Exception("Field name is required!");
			}
			
			if (!FieldType.TryParse(fieldTypeName, out var fieldType))
			{
				throw new Exception($"Field type missing or unsupported for '{fieldName}'");	
			}
			
			FieldEnumeratorInfo fieldEnumeratorInfo = null;
			if (fieldArrayEnumeratorSection != null)
			{
				fieldEnumeratorInfo = 
					FieldEnumeratorInfo.FromBase(
						ParseToFieldInfo(fieldArrayEnumeratorSection), 
						fieldArrayEnumeratorSection.GetValue<string>("item-container"));
			}

			FieldInfo[] fieldObjectSchema = default;
			if (fieldType.IsObject)
			{
				var objectSchemaSection = fieldSection.GetSection("schema");
				var objectSchemaFieldInfos = new List<FieldInfo>();
				var objectSchemaFieldSections = objectSchemaSection.GetChildren();
				foreach (var objectSchemaFieldSection in objectSchemaFieldSections)
				{
					objectSchemaFieldInfos.Add(ParseToFieldInfo(objectSchemaFieldSection));
				}

				fieldObjectSchema = objectSchemaFieldInfos.ToArray();
			}
				
			var fieldInfo = new FieldInfo
			{
				Name = fieldName,
				Description = fieldDescription,
				Route = fieldRoute,
				Type = fieldType,
				AttributeName = fieldAttributeName,
				Options = fieldOptions,
				Enumerator = fieldEnumeratorInfo,
				ObjectSchema = fieldObjectSchema
			};

			return fieldInfo;
		}

		#endregion
	}
}