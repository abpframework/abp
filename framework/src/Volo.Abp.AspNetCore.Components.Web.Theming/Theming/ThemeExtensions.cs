using System;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Theming;

public static class ThemeExtensions
{
    public static Type GetApplicationLayout(this ITheme theme, bool fallbackToDefault = true)
    {
        return theme.GetLayout(StandardLayouts.Application, fallbackToDefault);
    }

    public static Type GetAccountLayout(this ITheme theme, bool fallbackToDefault = true)
    {
        return theme.GetLayout(StandardLayouts.Account, fallbackToDefault);
    }

    public static Type GetPublicLayout(this ITheme theme, bool fallbackToDefault = true)
    {
        return theme.GetLayout(StandardLayouts.Public, fallbackToDefault);
    }

    public static Type GetEmptyLayout(this ITheme theme, bool fallbackToDefault = true)
    {
        return theme.GetLayout(StandardLayouts.Empty, fallbackToDefault);
    }
}
