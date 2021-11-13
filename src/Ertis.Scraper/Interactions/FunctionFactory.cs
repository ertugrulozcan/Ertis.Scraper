using System;
using System.Collections.Generic;
using System.Linq;

namespace Ertis.Scraper.Interactions
{
	public static class FunctionFactory
	{
		#region Fields

		private static IEnumerable<FunctionBase> _sampleInstances;

		#endregion
		
		#region Properties

		private static IEnumerable<FunctionBase> SampleInstances
		{
			get
			{
				if (_sampleInstances == null)
				{
					_sampleInstances = GenerateSampleInstances();
				}

				return _sampleInstances;
			}
		}

		#endregion

		#region Methods

		private static IEnumerable<FunctionBase> GenerateSampleInstances()
		{
			Func<System.Reflection.Assembly, Type[]> tryGetTypes = a =>
			{
				return a.IsDynamic ? new Type[] { } : a.GetTypes();
			};

			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => tryGetTypes(assembly)
					.Where(i => !i.IsAbstract && i.IsSubclassOf(typeof(FunctionBase))))
				.OrderBy(i => i.Assembly == typeof(FunctionBase).Assembly ? 0 : 1)
				.Select(type => (FunctionBase)Activator.CreateInstance(type));
		}

		public static FunctionBase CreateFunction(string name)
		{
			var sampling = SampleInstances.Single(x => x.Name == name);
			return (FunctionBase)Activator.CreateInstance(sampling.GetType());
		}
		
		public static bool TryCreateFunction(string name, out FunctionBase function)
		{
			try
			{
				function = CreateFunction(name);
				return true;
			}
			catch
			{
				function = null;
				return false;
			}
		}

		#endregion
	}
}