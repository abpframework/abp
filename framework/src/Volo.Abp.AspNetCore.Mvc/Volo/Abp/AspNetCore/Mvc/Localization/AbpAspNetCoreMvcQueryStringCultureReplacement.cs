using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpAspNetCoreMvcQueryStringCultureReplacement : IQueryStringCultureReplacement, ITransientDependency
{
    public virtual Task ReplaceAsync(QueryStringCultureReplacementContext context)
    {
        if (string.IsNullOrWhiteSpace(context.ReturnUrl))
        {
            return Task.CompletedTask;
        }

        var currentCulture = context.CurrentCulture
            ?? context.HttpContext.GetRouteValue("culture")?.ToString();

        if (!string.IsNullOrEmpty(currentCulture))
        {
            var escapedCulture = Regex.Escape(currentCulture);
            // Replace only the first occurrence so that paths like /en/products/en/details
            // only have the leading culture segment replaced, while tenant-prefixed paths
            // like /tenant-a/en/... are also handled correctly.
            var pattern = $"/{escapedCulture}(?=/|$|\\?|#)";
            context.ReturnUrl = new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(context.ReturnUrl, "/" + context.RequestCulture.Culture.Name, 1);
        }

        context.ReturnUrl = ReplaceQueryStringCulture(context.ReturnUrl, context);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Replaces <c>culture</c> and <c>ui-culture</c> query parameters in <paramref name="url"/>
    /// with the values from <paramref name="context"/>. Each parameter is handled independently —
    /// the presence of one does not require the other. Uses a proper query parser instead of
    /// regex to avoid false-positive matches inside other parameter values.
    /// </summary>
    protected virtual string ReplaceQueryStringCulture(string url, QueryStringCultureReplacementContext context)
    {
        var fragmentIndex = url.IndexOf('#');
        var fragment = fragmentIndex >= 0 ? url.Substring(fragmentIndex) : string.Empty;
        var urlWithoutFragment = fragmentIndex >= 0 ? url.Substring(0, fragmentIndex) : url;

        var queryIndex = urlWithoutFragment.IndexOf('?');
        if (queryIndex < 0)
        {
            return url;
        }

        var path = urlWithoutFragment.Substring(0, queryIndex);
        var queryString = urlWithoutFragment.Substring(queryIndex);
        var query = QueryHelpers.ParseQuery(queryString);

        if (!query.ContainsKey("culture") && !query.ContainsKey("ui-culture"))
        {
            return url;
        }

        if (query.ContainsKey("culture"))
        {
            query["culture"] = context.RequestCulture.Culture.Name;
        }

        if (query.ContainsKey("ui-culture"))
        {
            query["ui-culture"] = context.RequestCulture.UICulture.Name;
        }

        var rebuiltUrl = QueryHelpers.AddQueryString(
            path,
            query.SelectMany(kvp => kvp.Value.Select(v => KeyValuePair.Create<string, string?>(kvp.Key, v))));

        return rebuiltUrl + fragment;
    }
}
