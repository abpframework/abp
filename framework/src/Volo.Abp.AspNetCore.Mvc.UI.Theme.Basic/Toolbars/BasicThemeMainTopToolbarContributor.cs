using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
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

            var languageProvider = context.ServiceProvider.GetService<ILanguageProvider>();

            //TODO: This duplicates GetLanguages() usage. Can we eleminate this?
            if (languageProvider.GetLanguages().Count > 1)
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
