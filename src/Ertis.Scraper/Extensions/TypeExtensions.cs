using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Ertis.Scraper.Extensions
{
	public static class TypeExtensions
	{
		#region Methods

		public static object Parse(this Type type, [NotNull] string text, FieldFormatter formatter)
		{
			return type.Parse(text, formatter, CultureInfo.CurrentCulture);
		}
		
		public static object Parse(this Type type, [NotNull] string text, FieldFormatter formatter, [NotNull] CultureInfo cultureInfo)
		{
			if (type == typeof(int))
			{
				text = text.TrimAllWhitespaces();
				text = formatter.Format(text);
				return int.Parse(text, NumberStyles.Integer, cultureInfo);
			}
			else if (type == typeof(double))
			{
				text = text.TrimAllWhitespaces();
				text = formatter.Format(text);
				
				return double.Parse(text, 
					NumberStyles.Number | 
					NumberStyles.AllowThousands | 
					NumberStyles.AllowDecimalPoint | 
					NumberStyles.AllowLeadingSign |
					NumberStyles.AllowLeadingWhite |
					NumberStyles.AllowTrailingSign |
					NumberStyles.AllowTrailingWhite, 
					cultureInfo);
			}
			else if (type == typeof(bool))
			{
				text = formatter.Format(text);
				return bool.Parse(text);
			}
			else
			{
				text = formatter.Format(text);
				var converter = TypeDescriptor.GetConverter(type);
				
				// ReSharper disable once AssignNullToNotNullAttribute
				return converter.ConvertFromString(null, cultureInfo, text);
			}
		}

		public static bool TryParse(this Type type, [NotNull] string text, FieldFormatter formatter, out object value)
		{
			try
			{
				value = type.Parse(text, formatter);
				return true;
			}
			catch
			{
				value = default;
				return false;
			}
		}

		public static bool TryParse(this Type type, [NotNull] string text, FieldFormatter formatter, [NotNull] CultureInfo cultureInfo, out object value)
		{
			try
			{
				value = type.Parse(text, formatter, cultureInfo);
				return true;
			}
			catch
			{
				value = default;
				return false;
			}
		}

		#endregion
	}
}