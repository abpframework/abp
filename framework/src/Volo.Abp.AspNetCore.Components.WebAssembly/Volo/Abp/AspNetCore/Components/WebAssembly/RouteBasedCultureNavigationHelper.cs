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
        var slashIndex = relativePath.IndexOf('/');
        var firstSegment = slashIndex >= 0 ? relativePath.Substring(0, slashIndex) : relativePath;

        var allCultures = allLanguages.Select(l => l.CultureName);

        var newRelativePath = allCultures.Any(c => string.Equals(c, firstSegment, StringComparison.OrdinalIgnoreCase))
            ? newLanguage.CultureName + (slashIndex >= 0 ? relativePath.Substring(slashIndex) : string.Empty)
            : newLanguage.CultureName + "/" + relativePath;

        navigationManager.NavigateTo(navigationManager.ToAbsoluteUri(newRelativePath).ToString(), forceLoad: true);
        return Task.CompletedTask;
    }
}
