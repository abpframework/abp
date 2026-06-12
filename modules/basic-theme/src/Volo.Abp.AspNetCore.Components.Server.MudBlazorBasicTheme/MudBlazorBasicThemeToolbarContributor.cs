using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme.Themes.Basic;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;

namespace Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme;

public class MudBlazorBasicThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));
        }

        return Task.CompletedTask;
    }
}
