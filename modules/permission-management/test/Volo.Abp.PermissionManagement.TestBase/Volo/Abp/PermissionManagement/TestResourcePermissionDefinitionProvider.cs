using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class TestResourcePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup("TestEntityManagementPermissionGroup").AddPermission("TestEntityManagementPermission");

        context.AddResourcePermission("MyResourcePermission1", TestEntityResource.ResourceName, "TestEntityManagementPermission");
        context.AddResourcePermission("MyResourceDisabledPermission1", TestEntityResource.ResourceName, "TestEntityManagementPermission", isEnabled: false);
        context.AddResourcePermission("MyResourcePermission2", TestEntityResource.ResourceName, "TestEntityManagementPermission");
        context.AddResourcePermission("MyResourcePermission3", TestEntityResource.ResourceName, "TestEntityManagementPermission", multiTenancySide: MultiTenancySides.Host);
        context.AddResourcePermission("MyResourcePermission4", TestEntityResource.ResourceName, "TestEntityManagementPermission", multiTenancySide: MultiTenancySides.Host).WithProviders(UserPermissionValueProvider.ProviderName);

        var myPermission5 = context.AddResourcePermission("MyResourcePermission5", TestEntityResource.ResourceName, "TestEntityManagementPermission");
        myPermission5.StateCheckers.Add(new TestRequireRolePermissionStateProvider("super-admin"));

        context.AddResourcePermission("MyResourcePermission6", TestEntityResource.ResourceName, "TestEntityManagementPermission");

        context.AddResourcePermission("MyResourceDisabledPermission2", TestEntityResource.ResourceName, "TestEntityManagementPermission", isEnabled: false);

        context.AddResourcePermission("MyResourcePermission7", TestEntityResource.ResourceName, "TestEntityManagementPermission");
        context.AddResourcePermission("MyResourcePermission8", TestEntityResource.ResourceName, "TestEntityManagementPermission");
    }
}
