using System;
using Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.Themes.Basic;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme;

[ThemeName(Name)]
public class MudBlazorBasicTheme : ITheme, ITransientDependency
{
    public const string Name = "MudBlazorBasic";

    public virtual Type GetLayout(string name, bool fallbackToDefault = true)
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
