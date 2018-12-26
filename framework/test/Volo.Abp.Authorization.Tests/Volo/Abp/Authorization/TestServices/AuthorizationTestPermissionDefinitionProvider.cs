using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices
{
    public class AuthorizationTestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            PermissionGroupDefinition getGroup = context.GetGroupOrNull("TestGetGroup");
            if (getGroup == null)
            {
                getGroup = context.AddGroup("TestGetGroup");
            }
            PermissionGroupDefinition group = context.AddGroup("TestGroup");
            group.AddPermission("MyAuthorizedService1");
        }
    }
}
