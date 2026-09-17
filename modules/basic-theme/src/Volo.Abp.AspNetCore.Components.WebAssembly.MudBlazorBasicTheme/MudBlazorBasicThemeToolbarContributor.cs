using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;
using Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme.Themes.Basic;
using LoginDisplay = Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme.Themes.Basic.LoginDisplay;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme;

public class MudBlazorBasicThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));

            //TODO: Can we find a different way to understand if authentication was configured or not?
            var authenticationStateProvider = context.ServiceProvider
                .GetService<AuthenticationStateProvider>();

            if (authenticationStateProvider != null)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            }
        }

        return Task.CompletedTask;
    }
}
