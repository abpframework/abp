using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.MultiTenancy
{
    public class AbpTenantManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(TenantManagementPermissions.GroupName);

            var rolesPermission = identityGroup.AddPermission(TenantManagementPermissions.Tenants.Default);
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Create);
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Update);
            rolesPermission.AddChild(TenantManagementPermissions.Tenants.Delete);
        }
    }
}
