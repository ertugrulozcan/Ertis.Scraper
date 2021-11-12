using System;

namespace Ertis.Scraper.Annotations
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PseudoClassNameAttribute : Attribute
	{
		#region Properties

		public string FunctionName { get; }

		#endregion

		#region Constructors

		public PseudoClassNameAttribute(string name)
		{
			this.FunctionName = name;
		}

		#endregion
	}
}