using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(SettingManagementPermissions.GroupName, L("Permission:SettingManagement"));

            moduleGroup
                .AddPermission(SettingManagementPermissions.Emailing, L("Permission:Emailing"))
                .StateCheckers.Add(new AllowTenantsToChangeEmailSettingsFeatureSimpleStateChecker());
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpSettingManagementResource>(name);
        }
    }
}
