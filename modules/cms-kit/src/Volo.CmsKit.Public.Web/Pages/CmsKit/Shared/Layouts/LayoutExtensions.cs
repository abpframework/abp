using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Layouts;

public static class LayoutExtensions
{
    private static readonly Dictionary<string, Func<ITheme, string>> LayoutFunctions = new()
    {
        [StandardLayouts.Account] = theme => theme.GetAccountLayout(),
        [StandardLayouts.Public] = theme => theme.GetPublicLayout(),
        [StandardLayouts.Empty] = theme => theme.GetEmptyLayout(),
    };

    public static string GetLayoutByKey(this ITheme theme, string layoutKey)
    {
        return !string.IsNullOrWhiteSpace(layoutKey) && LayoutFunctions.TryGetValue(layoutKey, out var layoutFunc)
           ? layoutFunc(theme)
           : theme.GetApplicationLayout(); // Application layout is the default
    }
}