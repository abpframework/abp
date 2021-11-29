using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement
{
    public class FeaturePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var featureManagementGroup = context.AddGroup(
                FeatureManagementPermissions.GroupName,
                L("Permission:FeatureManagement"),
                multiTenancySide: MultiTenancySides.Host);

            featureManagementGroup.AddPermission(
                FeatureManagementPermissions.ManageHostFeatures,
                L("Permission:FeatureManagement.ManageHostFeatures"),
                multiTenancySide: MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFeatureManagementResource>(name);
        }
    }
}
