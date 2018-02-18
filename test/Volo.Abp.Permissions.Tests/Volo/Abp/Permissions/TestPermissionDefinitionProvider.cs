using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Permissions
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var testGroup = context.AddGroup("TestGroup");

            testGroup.AddPermission("MyPermission1");

            var myPermission2 = testGroup.AddPermission("MyPermission2");
            myPermission2.AddChild("MyPermission2.ChildPermission1");
        }
    }
}