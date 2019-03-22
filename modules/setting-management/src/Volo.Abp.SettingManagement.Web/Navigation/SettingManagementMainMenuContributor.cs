using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement.Localization;
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

            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();

            context.Menu
                .GetAdministration()
                .AddItem(
                    new ApplicationMenuItem(
                        SettingManagementMenuNames.GroupName,
                        l["Settings"],
                        "/SettingManagement",
                        icon: "fa fa-cog"
                    )
                );

            return Task.CompletedTask;
        }
    }
}
