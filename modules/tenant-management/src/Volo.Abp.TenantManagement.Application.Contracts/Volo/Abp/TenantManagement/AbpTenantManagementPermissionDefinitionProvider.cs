using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement
{
    public class AbpTenantManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var tenantManagementGroup = context.AddGroup(TenantManagementPermissions.GroupName, L("Permission:TenantManagement"));

            var tenantsPermission = tenantManagementGroup.AddPermission(TenantManagementPermissions.Tenants.Default, L("Permission:TenantManagement"));
            tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Create, L("Permission:Create"));
            tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Update, L("Permission:Edit"));
            tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Delete, L("Permission:Delete"));
            tenantsPermission.AddChild(TenantManagementPermissions.Tenants.ManageFeatures, L("Permission:ManageFeatures"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpTenantManagementResource>(name);
        }
    }
}
