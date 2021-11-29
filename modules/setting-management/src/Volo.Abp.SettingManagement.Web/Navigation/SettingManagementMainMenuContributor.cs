﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.UI.Navigation;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Web.Navigation
{
    public class SettingManagementMainMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            if (context.Menu.FindMenuItem(SettingManagementMenuNames.GroupName) != null)
            {
                /* This may happen if blazor server UI is being used in the same application.
                 * In this case, we don't add the MVC setting management UI. */
                return;
            }

            var settingManagementPageOptions = context.ServiceProvider.GetRequiredService<IOptions<SettingManagementPageOptions>>().Value;
            var settingPageCreationContext = new SettingPageCreationContext(context.ServiceProvider);
            if (!settingManagementPageOptions.Contributors.Any() ||
                !(await CheckAnyOfPagePermissionsGranted(settingManagementPageOptions, settingPageCreationContext)))
            {
                return;
            }

            var l = context.GetLocalizer<AbpSettingManagementResource>();

            context.Menu
                .GetAdministration()
                .AddItem(
                    new ApplicationMenuItem(
                        SettingManagementMenuNames.GroupName,
                        l["Settings"],
                        "~/SettingManagement",
                        icon: "fa fa-cog"
                    ).RequireFeatures(SettingManagementFeatures.Enable)
                );
        }

        protected virtual async Task<bool> CheckAnyOfPagePermissionsGranted(
            SettingManagementPageOptions settingManagementPageOptions,
            SettingPageCreationContext settingPageCreationContext)
        {
            foreach (var contributor in settingManagementPageOptions.Contributors)
            {
                if (await contributor.CheckPermissionsAsync(settingPageCreationContext))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
