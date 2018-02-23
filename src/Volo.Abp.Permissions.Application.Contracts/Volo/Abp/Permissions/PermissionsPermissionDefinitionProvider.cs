using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Permissions
{
    public class PermissionsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(PermissionPermissions.GroupName);

            var permissions = group.AddPermission(PermissionPermissions.Permissions.Default);
            permissions.AddChild(PermissionPermissions.Permissions.Update);
        }
    }
}
