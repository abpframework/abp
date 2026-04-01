using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// A route constraint that only matches culture values configured in
/// <see cref="AbpLocalizationOptions.Languages"/>.
/// </summary>
public class AbpCultureRouteConstraint : IRouteConstraint
{
    public virtual bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var value) || value is not string cultureValue)
        {
            return false;
        }

        var languages = httpContext?.RequestServices
            .GetService<IOptions<AbpLocalizationOptions>>()?.Value.Languages;

        if (languages == null || languages.Count == 0)
        {
            // During URL generation, HttpContext or services may not be available.
            return routeDirection == RouteDirection.UrlGeneration;
        }

        return languages.Any(l =>
            string.Equals(l.CultureName, cultureValue, StringComparison.OrdinalIgnoreCase));
    }
}
