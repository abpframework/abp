using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.SettingManagement.Web.Navigation
{
    public class SettingManagementMainMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var settingManagementPageOptions = context.ServiceProvider.GetRequiredService<IOptions<SettingManagementPageOptions>>().Value;
            if (!settingManagementPageOptions.Contributors.Any())
            {
                return Task.CompletedTask;
            }

            //TODO: Localize
            //var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();

            context.Menu.AddItem(new ApplicationMenuItem("Volo.Abp.SettingManagement", "Settings", "/SettingManagement", icon: "fa fa-cog", order: int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}
