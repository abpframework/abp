using Shouldly;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices;

public class AuthorizationTestPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var getGroup = context.GetGroupOrNull("TestGetGroup");
        if (getGroup == null)
        {
            getGroup = context.AddGroup("TestGetGroup");
        }

        var group = context.AddGroup("TestGroup");

        group.AddPermission("MyAuthorizedService1");

        group.AddPermission("MyPermission1").StateCheckers.Add(new TestRequireEditionPermissionSimpleStateChecker());
        group.AddPermission("MyPermission2");
        group.AddPermission("MyPermission3");
        group.AddPermission("MyPermission4");
        group.AddPermission("MyPermission5");
        group.AddPermission("MyPermission6").WithProviders(nameof(TestPermissionValueProvider1));
        group.AddPermission("MyPermission7").WithProviders(nameof(TestPermissionValueProvider2));

        group.GetPermissionOrNull("MyAuthorizedService1").ShouldNotBeNull();

        context.RemoveGroup("TestGetGroup");
    }
}
