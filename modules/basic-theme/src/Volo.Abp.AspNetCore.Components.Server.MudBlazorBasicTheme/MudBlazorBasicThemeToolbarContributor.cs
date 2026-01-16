using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;
using Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.Themes.Basic;

namespace Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme;

public class MudBlazorBasicThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
        }

        return Task.CompletedTask;
    }
}
