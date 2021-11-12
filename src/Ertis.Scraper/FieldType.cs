using System;
using System.Diagnostics.CodeAnalysis;

namespace Ertis.Scraper
{
	public readonly struct FieldType
	{
		#region Statics

		public static readonly FieldType Integer = new() { BaseType = typeof(int) };
		public static readonly FieldType Double = new() { BaseType = typeof(double) };
		public static readonly FieldType Character = new() { BaseType = typeof(char) };
		public static readonly FieldType Boolean = new() { BaseType = typeof(bool) };
		public static readonly FieldType String = new() { BaseType = typeof(string) };
		public static readonly FieldType Array = new() { BaseType = typeof(object[]) };
		public static readonly FieldType Object = new() { BaseType = typeof(object) };

		#endregion
		
		#region Properties

		[NotNull]
		public Type BaseType { get; private init; }

		public bool IsInteger => this.BaseType == typeof(int);
		public bool IsDouble => this.BaseType == typeof(double);
		public bool IsCharacter => this.BaseType == typeof(char);
		public bool IsBoolean => this.BaseType == typeof(bool);
		public bool IsString => this.BaseType == typeof(string);
		public bool IsArray => this.BaseType == typeof(object[]);
		public bool IsObject => this.BaseType == typeof(object);

		#endregion

		#region Methods

		public static bool TryParse(string typeName, out FieldType type)
		{
			try
			{
				type = Parse(typeName);
				return true;
			}
			catch
			{
				type = default;
				return false;
			}
		}
		
		public static FieldType Parse(string typeName)
		{
			return typeName switch
			{
				"int" => Integer,
				"double" => Double,
				"char" => Character,
				"bool" => Boolean,
				"string" => String,
				"array" => Array,
				"object" => Object,
				_ => throw new Exception($"Unsupported field type! ('{typeName}')")
			};
		}

		public override string ToString()
		{
			return this.BaseType.Name;
		}

		public override bool Equals(object obj)
		{
			if (obj is FieldType other)
			{
				return this.BaseType == other.BaseType;
			}
			else
			{
				return false;
			}
		}

		public bool Equals(FieldType other)
		{
			return this.BaseType == other.BaseType;
		}

		public override int GetHashCode()
		{
			return (this.BaseType != null ? this.BaseType.GetHashCode() : 0);
		}

		#endregion
	}
}