using System.Linq;

namespace Ertis.Scraper.Interactions
{
	public abstract class FunctionBase
	{
		#region Fields

		private FunctionParameter[] parameters;

		#endregion
		
		#region Properties

		public abstract string Name { get; }
		
		protected abstract FunctionParameter[] DefaultParameters { get; }

		public FunctionParameter[] Parameters
		{
			get
			{
				if (this.parameters == null && this.DefaultParameters != null)
				{
					this.parameters = this.DefaultParameters;
				}

				return this.parameters;
			}
		}

		#endregion

		#region Methods

		protected internal T GetParameterValue<T>(string parameterName)
		{
			if (this.Parameters.Single(x => x.Name == parameterName) is FunctionParameter<T> parameter)
			{
				return parameter.Value;
			}

			return default;
		}

		#endregion
	}
}