using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class RouteBasedCultureUrlHelper : IRouteBasedCultureUrlHelper, ITransientDependency
{
    private readonly ICachedApplicationConfigurationClient _configurationClient;

    public RouteBasedCultureUrlHelper(ICachedApplicationConfigurationClient configurationClient)
    {
        _configurationClient = configurationClient;
    }

    public virtual async Task<string> PrependCulturePrefixAsync(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return url;
        }

        // Skip absolute URLs with a web scheme and protocol-relative URLs.
        // Intentionally avoids Uri.TryCreate here: on Unix, root-relative paths such as
        // "/account/login" are parsed as absolute file:// URIs, which would incorrectly
        // skip them before the culture prefix could be applied.
        if (url.StartsWith("//", StringComparison.Ordinal) ||
            url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        var config = await _configurationClient.GetAsync();
        if (config?.Localization.UseRouteBasedCulture != true)
        {
            return url;
        }

        var currentCulture = CultureInfo.CurrentCulture.Name;
        var isKnownCulture = config.Localization.Languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        if (!isKnownCulture)
        {
            return url;
        }

        // Idempotency guard: if the URL already carries the culture prefix, return it unchanged.
        // Strip the leading scheme prefix (~/  or  /) before checking the first path segment.
        var pathForSegmentCheck = url.StartsWith("~/", StringComparison.Ordinal) ? url.Substring(2)
                                : url.StartsWith("/", StringComparison.Ordinal) ? url.Substring(1)
                                : url;

        if (string.Equals(GetFirstPathSegment(pathForSegmentCheck),
                currentCulture, StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        if (url.StartsWith("~/", StringComparison.Ordinal))
        {
            return "~/" + currentCulture + "/" + url.Substring(2);
        }

        if (url.StartsWith("/", StringComparison.Ordinal))
        {
            return "/" + currentCulture + url;
        }

        // Bare relative path (e.g. "authentication/login")
        return currentCulture + "/" + url;
    }

    /// <summary>
    /// Returns the first path segment of <paramref name="baseRelativePath"/>,
    /// stripping any query string or fragment before splitting on '/'.
    /// For example: "zh-Hans/account?x=1" → "zh-Hans", "tr/home#top" → "tr".
    /// </summary>
    protected virtual string GetFirstPathSegment(string baseRelativePath)
    {
        var suffixIndex = baseRelativePath.IndexOfAny(['?', '#']);
        var pathPart = suffixIndex >= 0 ? baseRelativePath.Substring(0, suffixIndex) : baseRelativePath;
        var slashIndex = pathPart.IndexOf('/');
        return slashIndex >= 0 ? pathPart.Substring(0, slashIndex) : pathPart;
    }
}
