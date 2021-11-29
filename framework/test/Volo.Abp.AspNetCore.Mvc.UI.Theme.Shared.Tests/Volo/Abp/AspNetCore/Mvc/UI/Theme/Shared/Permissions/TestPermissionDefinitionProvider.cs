using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Tests.Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Permissions
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup("TestGroup");

            group.AddPermission("MyComponent1");
            group.AddPermission("MyComponent2");
            group.AddPermission("MyComponent3");
        }
    }
}
