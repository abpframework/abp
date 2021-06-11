using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup;

namespace Volo.Abp.SettingManagement.Web.Settings
{
    public class EmailingPageContributor: ISettingPageContributor
    {
        public async Task ConfigureAsync(SettingPageCreationContext context)
        {
            if (!await CheckPermissionsInternalAsync(context))
            {
                return;
            }

            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();
            context.Groups.Add(
                new SettingPageGroup(
                    "Volo.Abp.EmailSetting",
                    l["Menu:Emailing"],
                    typeof(EmailSettingGroupViewComponent)
                )
            );
        }

        public async Task<bool> CheckPermissionsAsync(SettingPageCreationContext context)
        {
            return await CheckPermissionsInternalAsync(context);
        }

        private async Task<bool> CheckPermissionsInternalAsync(SettingPageCreationContext context)
        {
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

            return await authorizationService.IsGrantedAsync(SettingManagementPermissions.Emailing);
        }
    }
}
