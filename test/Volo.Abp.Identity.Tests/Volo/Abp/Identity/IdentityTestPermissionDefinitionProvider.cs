using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Identity
{
    public class IdentityTestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var testGroup = context.AddGroup(TestPermissionNames.Groups.TestGroup);

            testGroup.AddPermission(TestPermissionNames.MyPermission1);

            var myPermission2 = testGroup.AddPermission(TestPermissionNames.MyPermission2);
            myPermission2.AddChild(TestPermissionNames.MyPermission2_ChildPermission1);
        }
    }
}
