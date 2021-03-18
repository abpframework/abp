using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Settings
{
    public class EmailingPageContributor: ISettingComponentContributor
    {
        public async Task ConfigureAsync(SettingComponentCreationContext context)
        {
            if (!await CheckPermissionsInternalAsync(context))
            {
                return;
            }

            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();
            context.Groups.Add(
                new SettingComponentGroup(
                    "Volo.Abp.SettingManagement",
                    l["Menu:Emailing"],
                    typeof(EmailSettingGroupViewComponent)
                )
            );
        }

        public async Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
        {
            return await CheckPermissionsInternalAsync(context);
        }

        private async Task<bool> CheckPermissionsInternalAsync(SettingComponentCreationContext context)
        {
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

            return await authorizationService.IsGrantedAsync(SettingManagementPermissions.Emailing);
        }
    }
}
