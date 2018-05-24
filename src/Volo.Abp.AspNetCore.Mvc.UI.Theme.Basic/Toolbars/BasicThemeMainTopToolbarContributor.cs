using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

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

            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));

            return Task.CompletedTask;
        }
    }
}
