using System;
using System.Collections.Generic;
using System.Linq;

namespace Ertis.Scraper
{
	public class SelectorToken
	{
		#region Properties

		public string Filter { get; set; }
		
		public IList<SelectorToken> SubTokens { get; set; }

		#endregion
		
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="token"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public SelectorToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				throw new ArgumentNullException(nameof(token));
			}

			var tokens = SplitTokens(token).ToList();

			this.Filter = tokens.FirstOrDefault();
			this.SubTokens = tokens.Skip(1).Select(i => new SelectorToken(i)).ToList();
		}

		#endregion
		
		#region Methods

		private static IEnumerable<string> SplitTokens(string token)
		{
			if (token.StartsWith(XPathSelector.XPathSelectorToken))
			{
				return new[] { token };
			}
			
			Func<char, bool> isNameToken = c => char.IsLetterOrDigit(c) || c is '-' or '_';
			var tokens = new List<string>();
           
			int start = 0;
			bool isPrefix = true;
			bool isOpeningBracket = false;
			char closeBracket = '\0';
			for (int i = 0; i < token.Length; i++)
			{
				if (isOpeningBracket && token[i] != closeBracket)
					continue;

				isOpeningBracket = false;

				if (token[i] == '(')
				{
					closeBracket = ')';
					isOpeningBracket = true;
				}
				else if (token[i] == '[')
				{
					closeBracket = ']';
					if (i != start)
					{
						tokens.Add(token.Substring(start, i - start));
						start = i;
					}
					
					isOpeningBracket = true;
				}
				else if (i == token.Length - 1)
				{
					tokens.Add(token.Substring(start, i - start + 1));
				}
				else if (!isNameToken(token[i]) && !isPrefix)
				{
					tokens.Add(token.Substring(start, i - start));
					start = i;
				}
				else if (isNameToken(token[i]))
				{
					isPrefix = false;
				}
			}

			return tokens;
		}

		#endregion
	}
}