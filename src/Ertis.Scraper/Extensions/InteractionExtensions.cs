using System;
using Ertis.Scraper.Interactions;
using PuppeteerSharp;

namespace Ertis.Scraper.Extensions
{
	public static class InteractionExtensions
	{
		#region Methods

		internal static CookieParam GetCookieParam(this FunctionBase function)
		{
			var sameSite = SameSite.None;
			var cookiePolicy = function.GetParameterValue<CookiePolicy?>("sameSite");
			if (cookiePolicy != null)
			{
				sameSite = cookiePolicy.Value switch
				{
					CookiePolicy.Strict => SameSite.Strict,
					CookiePolicy.Lax => SameSite.Lax,
					CookiePolicy.Extended => SameSite.Extended,
					CookiePolicy.None => SameSite.None,
					_ => throw new ArgumentOutOfRangeException()
				};
			}
			
			var cookie = new CookieParam
			{
				Domain = function.GetParameterValue<string>("domain"),
				Expires = function.GetParameterValue<int>("expires"),
				Name = function.GetParameterValue<string>("name"),
				Path = function.GetParameterValue<string>("path"),
				Secure = function.GetParameterValue<bool>("secure"),
				Session = function.GetParameterValue<bool>("session"),
				Size = function.GetParameterValue<int>("size"),
				Url = function.GetParameterValue<string>("url"),
				Value = function.GetParameterValue<string>("value"),
				HttpOnly = function.GetParameterValue<bool>("httpOnly"),
				SameSite = sameSite
			};

			return cookie;
		}

		#endregion
	}
}