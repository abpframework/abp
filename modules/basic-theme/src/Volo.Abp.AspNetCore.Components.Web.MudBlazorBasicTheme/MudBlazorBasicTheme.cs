using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme;

public class MudBlazorBasicTheme : ITheme
{
    public static string Name = "MudBlazorBasic";

    public string GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
                return "~/Themes/Basic/MainLayout";
            case StandardLayouts.Account:
                return "~/Themes/Basic/MainLayout";
            case StandardLayouts.Empty:
                return "~/Themes/Basic/NullLayout";
            default:
                return fallbackToDefault ? "~/Themes/Basic/MainLayout" : null;
        }
    }

    public string GetStyle(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardStyles.Global:
                return "~/themes/basic/main.css";
            default:
                return fallbackToDefault ? "~/themes/basic/main.css" : null;
        }
    }
}
