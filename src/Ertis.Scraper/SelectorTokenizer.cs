using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ertis.Scraper
{
	public static class SelectorTokenizer
	{
		#region Methods

		public static IEnumerable<SelectorToken> GetTokens(string cssFilter)
		{
			var reader = new StringReader(cssFilter);
			while (true)
			{
				int v = reader.Read();
				if (v < 0)
					yield break;

				char c = (char)v;
				if (c == '>')
				{
					yield return new SelectorToken(">");
					continue;
				}

				if (c is ' ' or '\t')
					continue;

				string word = c + ReadWord(reader);
				yield return new SelectorToken(word);
			}
		}

		private static string ReadWord(TextReader reader)
		{
			var stringBuilder = new StringBuilder();
			while (true)
			{
				int v = reader.Read();
				if (v < 0)
					break;

				char c = (char)v;
				if (c is ' ' or '\t')
					break;

				stringBuilder.Append(c);
			}

			return stringBuilder.ToString();
		}

		#endregion
	}
}