using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var testGroup = context.AddGroup("TestGroup");

            testGroup.AddPermission("MyPermission1");
            testGroup.AddPermission("MyDisabledPermission1", isEnabled: false);

            var myPermission2 = testGroup.AddPermission("MyPermission2");
            myPermission2.AddChild("MyPermission2.ChildPermission1");

            testGroup.AddPermission("MyPermission3", multiTenancySide: MultiTenancySides.Host);

            testGroup.AddPermission("MyPermission4", multiTenancySide: MultiTenancySides.Host).WithProviders(UserPermissionValueProvider.ProviderName);

            testGroup.AddPermission("MyPermission5").StateCheckers.Add(new TestRequireRolePermissionStateProvider("super-admin"));
        }
    }
}
