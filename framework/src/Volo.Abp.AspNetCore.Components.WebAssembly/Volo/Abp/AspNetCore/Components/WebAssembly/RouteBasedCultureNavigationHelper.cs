using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class RouteBasedCultureNavigationHelper : IRouteBasedCultureNavigationHelper, ITransientDependency
{
    public virtual Task NavigateToNewCultureAsync(
        NavigationManager navigationManager,
        LanguageInfo newLanguage,
        IEnumerable<LanguageInfo> allLanguages)
    {
        var relativePath = navigationManager.ToBaseRelativePath(navigationManager.Uri);

        // Separate the path from any query string or fragment so the culture segment
        // is correctly identified even for URLs like "tr?x=1" (no slash after culture).
        var suffixIndex = relativePath.IndexOfAny(['?', '#']);
        var pathPart = suffixIndex >= 0 ? relativePath.Substring(0, suffixIndex) : relativePath;
        var suffix = suffixIndex >= 0 ? relativePath.Substring(suffixIndex) : string.Empty;

        var slashIndex = pathPart.IndexOf('/');
        var firstSegment = GetFirstPathSegment(relativePath);
        var pathRemainder = slashIndex >= 0 ? pathPart.Substring(slashIndex) : string.Empty;

        // No-op: the current URL already shows the target culture — no navigation needed.
        if (string.Equals(firstSegment, newLanguage.CultureName, StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        var newRelativePath = allLanguages.Any(l => string.Equals(l.CultureName, firstSegment, StringComparison.OrdinalIgnoreCase))
            ? newLanguage.CultureName + pathRemainder + suffix
            : newLanguage.CultureName + "/" + pathPart + suffix;

        navigationManager.NavigateTo(navigationManager.ToAbsoluteUri(newRelativePath).ToString(), forceLoad: true);
        return Task.CompletedTask;
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
