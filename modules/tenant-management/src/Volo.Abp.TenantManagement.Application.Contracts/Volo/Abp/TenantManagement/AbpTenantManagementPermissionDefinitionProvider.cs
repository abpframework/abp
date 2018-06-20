using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement
{
    public class AbpTenantManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(TenantManagementPermissions.GroupName, L("Permission:TenantManagement"));

            var rolesPermission = identityGroup.AddPermission(TenantManagementPermissions.Tenants.Default, L("Permission:TenantManagement"));
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Create, L("Permission:Create"));
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Update, L("Permission:Edit"));
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpTenantManagementResource>(name);
        }
    }
}
