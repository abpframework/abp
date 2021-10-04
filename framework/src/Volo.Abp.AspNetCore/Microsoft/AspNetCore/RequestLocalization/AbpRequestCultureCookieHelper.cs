using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public static class AbpRequestCultureCookieHelper
    {
        public static void SetCultureCookie(
            HttpContext httpContext,
            RequestCulture requestCulture)
        {
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(requestCulture),
                new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTime.Now.AddYears(2)
                }
            );
        }
    }
}
