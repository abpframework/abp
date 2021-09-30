using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var testGroup = context.AddGroup("TestGroup");

            testGroup.AddPermission("TestPermission1");
            testGroup.AddPermission("TestPermission2");
        }
    }
}
