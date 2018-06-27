using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Toolbars
{
    public class BasicThemeMainTopToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return Task.CompletedTask;
            }

            if (!(context.Theme is BasicTheme))
            {
                return Task.CompletedTask;
            }

            var requestLocalizationOptions = context.ServiceProvider.GetService<IOptions<RequestLocalizationOptions>>();

            if (requestLocalizationOptions.Value.SupportedCultures.Count > 1)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
            }

            if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));
            }

            return Task.CompletedTask;
        }
    }
}
