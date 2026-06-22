using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.RequestLocalization;

public static class AbpRequestCultureCookieHelper
{
    public const string HasRouteCultureCookieName = "Abp.HasRouteCulture";

    /// <summary>
    /// Gets the current route culture from the request. First checks route values,
    /// then falls back to the HasRouteCulture cookie (set during Blazor SSR) with CurrentCulture.
    /// Returns null if the request has no route-based culture.
    /// </summary>
    public static string? GetRouteCulture(HttpContext? httpContext)
    {
        if (httpContext == null)
        {
            return null;
        }

        var routeCulture = httpContext.GetRouteValue("culture")?.ToString();
        if (!string.IsNullOrEmpty(routeCulture))
        {
            return routeCulture;
        }

        if (httpContext.Request.Cookies.ContainsKey(HasRouteCultureCookieName))
        {
            return CultureInfo.CurrentCulture.Name;
        }

        return null;
    }

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

    public static void SetHasRouteCultureCookie(HttpContext httpContext, bool hasRouteCulture)
    {
        if (hasRouteCulture)
        {
            httpContext.Response.Cookies.Append(
                HasRouteCultureCookieName, "1",
                new CookieOptions
                {
                    IsEssential = true
                });
        }
        else
        {
            httpContext.Response.Cookies.Delete(HasRouteCultureCookieName);
        }
    }
}
