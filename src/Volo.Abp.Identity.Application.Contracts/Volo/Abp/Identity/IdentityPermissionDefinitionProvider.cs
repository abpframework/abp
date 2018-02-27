using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Identity
{
    public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(IdentityPermissions.GroupName);

            var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default);
            rolesPermission.AddChild(IdentityPermissions.Roles.Create);
            rolesPermission.AddChild(IdentityPermissions.Roles.Update);
            rolesPermission.AddChild(IdentityPermissions.Roles.Delete);
            rolesPermission.AddChild(IdentityPermissions.Roles.ManagePermissions);

            var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default);
            usersPermission.AddChild(IdentityPermissions.Users.Create);
            usersPermission.AddChild(IdentityPermissions.Users.Update);
            usersPermission.AddChild(IdentityPermissions.Users.Delete);
            usersPermission.AddChild(IdentityPermissions.Users.ManagePermissions);
        }
    }
}
