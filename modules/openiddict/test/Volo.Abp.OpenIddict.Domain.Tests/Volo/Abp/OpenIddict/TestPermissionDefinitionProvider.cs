using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.OpenIddict;

public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var testGroup = context.AddGroup(TestPermissionNames.Groups.TestGroup);
        testGroup.AddPermission(TestPermissionNames.MyPermission1);
        testGroup.AddPermission(TestPermissionNames.MyPermission2);
    }
}
