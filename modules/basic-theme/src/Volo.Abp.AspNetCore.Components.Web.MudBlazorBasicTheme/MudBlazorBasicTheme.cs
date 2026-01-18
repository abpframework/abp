using System;
using Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.Themes.Basic;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Theming;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme;

public class MudBlazorBasicTheme : ITheme
{
    public static string Name = "MudBlazorBasic";

    public Type GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
            case StandardLayouts.Account:
            case StandardLayouts.Empty:
                return typeof(MainLayout);
            default:
                return fallbackToDefault ? typeof(MainLayout) : typeof(NullLayout);
        }
    }
}
