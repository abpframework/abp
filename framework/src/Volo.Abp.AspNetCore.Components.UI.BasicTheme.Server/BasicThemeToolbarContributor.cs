using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.UI.BasicTheme.Themes.Basic;
using Volo.Abp.AspNetCore.Components.UI.Theming.Toolbars;

namespace Volo.Abp.AspNetCore.Components.UI.BasicTheme.Server
{
    public class BasicThemeToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == StandardToolbars.Main)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            }

            return Task.CompletedTask;
        }
    }
}
