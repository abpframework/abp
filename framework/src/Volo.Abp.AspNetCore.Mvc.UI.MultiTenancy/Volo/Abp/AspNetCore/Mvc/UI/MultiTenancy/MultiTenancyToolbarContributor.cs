using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy.Components.TenantSwitch;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy
{
    public class MultiTenancyToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return Task.CompletedTask;
            }

            context.Toolbar.Items.Add(new ToolbarItem(typeof(TenantSwitchViewComponent), TenantSwitchViewComponent.Order));

            return Task.CompletedTask;
        }
    }
}
