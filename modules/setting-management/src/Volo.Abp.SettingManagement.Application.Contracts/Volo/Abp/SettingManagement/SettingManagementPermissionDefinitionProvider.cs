using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(SettingManagementPermissions.GroupName, L("Permission:SettingManagement"));
            var emailingPermission = moduleGroup.AddPermission(SettingManagementPermissions.Emailing, L("Permission:Emailing"));

            if (IsTenantAvailable(context))
            {
                emailingPermission.RequireFeatures(SettingManagementFeatures.AllowTenantsToChangeEmailSettings);
            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpSettingManagementResource>(name);
        }

        private static bool IsTenantAvailable(IPermissionDefinitionContext context)
        {
            var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

            return currentTenant.IsAvailable;
        }
    }
}
