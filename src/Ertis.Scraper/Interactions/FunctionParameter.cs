using System;

namespace Ertis.Scraper.Interactions
{
	public abstract class FunctionParameter
	{
		#region Properties

		public string Name { get; init; }
		
		public abstract Type Type { get; }
		
		#endregion

		#region Methods

		public abstract void SetValue(object value);

		#endregion
	}
	
	public class FunctionParameter<T> : FunctionParameter
	{
		#region Properties

		public T Value { get; private set; }

		public override Type Type => typeof(T);

		#endregion

		#region Methods

		public override void SetValue(object value)
		{
			this.Value = (T) value;
		}

		#endregion
	}
}