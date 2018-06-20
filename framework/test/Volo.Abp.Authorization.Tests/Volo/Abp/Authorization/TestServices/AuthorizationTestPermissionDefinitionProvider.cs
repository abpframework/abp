using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices
{
    public class AuthorizationTestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup("TestGroup");
            group.AddPermission("MyAuthorizedService1");
        }
    }
}
