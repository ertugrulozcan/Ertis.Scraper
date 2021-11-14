using Ertis.Scraper.Extensions;

namespace Ertis.Scraper
{
	public class FieldFormatter
	{
		#region Properties

		public FieldFormatting Options { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
		public FieldFormatter(FieldFormatting options)
		{
			this.Options = options;
		}

		#endregion

		#region Methods

		public string Format(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			
			if (this.Options.TrimStart != null || this.Options.TrimEnd != null)
			{
				if (this.Options.TrimStart == string.Empty && this.Options.TrimEnd == string.Empty)
				{
					value = value.Trim();
				}
				else
				{
					value = value.TrimStart(this.Options.TrimStart).TrimEnd(this.Options.TrimEnd);
				}
			}

			if (!string.IsNullOrEmpty(this.Options.AppendStart))
			{
				value = this.Options.AppendStart + value;
			}

			if (!string.IsNullOrEmpty(this.Options.AppendEnd))
			{
				value += this.Options.AppendEnd;
			}

			return value;
		}

		#endregion
	}
}