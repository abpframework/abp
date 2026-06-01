using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.TestServices.Resources;
using Volo.Abp.StaticDefinitions;
using Xunit;

namespace Volo.Abp.Authorization;

public class StaticPermissionDefinitionStore_Tests : AuthorizationTestBase
{
    private readonly IStaticPermissionDefinitionStore _store;

    public StaticPermissionDefinitionStore_Tests()
    {
        _store = GetRequiredService<IStaticPermissionDefinitionStore>();
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        var permission = await _store.GetOrNullAsync("MyPermission1");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyPermission1");
        permission.StateCheckers.ShouldContain(x => x.GetType() == typeof(TestRequireEditionPermissionSimpleStateChecker));

        permission = await _store.GetOrNullAsync("NotExists");
        permission.ShouldBeNull();
    }

    [Fact]
    public async Task GetPermissionsAsync()
    {
        var permissions = await _store.GetPermissionsAsync();
        permissions.ShouldContain(x => x.Name == "MyAuthorizedService1");
        permissions.ShouldContain(x => x.Name == "MyPermission1");
        permissions.ShouldContain(x => x.Name == "MyPermission2");
        permissions.ShouldContain(x => x.Name == "MyPermission3");
        permissions.ShouldContain(x => x.Name == "MyPermission4");
        permissions.ShouldContain(x => x.Name == "MyPermission5");
    }

    [Fact]
    public async Task GetGroupsAsync()
    {
        var groups = await _store.GetGroupsAsync();
        groups.ShouldNotContain(x => x.Name == "TestGetGroup");
    }

    [Fact]
    public async Task GetResourcePermissionOrNullAsync()
    {
        var permission = await _store.GetResourcePermissionOrNullAsync(TestEntityResource.ResourceName, "MyResourcePermission1");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyResourcePermission1");
        permission.StateCheckers.ShouldContain(x => x.GetType() == typeof(TestRequireEditionPermissionSimpleStateChecker));

        permission = await _store.GetResourcePermissionOrNullAsync(TestEntityResource.ResourceName, "NotExists");
        permission.ShouldBeNull();
    }

    [Fact]
    public async Task GetResourcePermissionsAsync()
    {
        var permissions = await _store.GetResourcePermissionsAsync();
        permissions.ShouldContain(x => x.Name == "MyResourcePermission1");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission2");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission3");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission4");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission5");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission6");
        permissions.ShouldContain(x => x.Name == "MyResourcePermission7");
    }

    [Fact]
    public async Task GetResourcePermissionsAsync_Same_Name_In_Different_Resources_Should_Both_Be_Returned()
    {
        // MyResourcePermission7 is defined for both TestEntityResource and TestEntityResource2.
        // GetResourcePermissionsAsync must return both entries because resource permissions are
        // unique by (ResourceName, Name), not by Name alone.
        var permissions = await _store.GetResourcePermissionsAsync();

        permissions.ShouldContain(x =>
            x.Name == "MyResourcePermission7" && x.ResourceName == TestEntityResource.ResourceName);
        permissions.ShouldContain(x =>
            x.Name == "MyResourcePermission7" && x.ResourceName == TestEntityResource2.ResourceName);
    }

    [Fact]
    public async Task Should_Rebuild_Definitions_In_Fresh_ExecutionContext_After_Cache_Clear()
    {
        var groupCache = GetRequiredService<IStaticDefinitionCache<PermissionGroupDefinition,
            (Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)>>();
        var definitionCache = GetRequiredService<IStaticDefinitionCache<PermissionDefinition,
            Dictionary<string, PermissionDefinition>>>();

        await groupCache.ClearAsync();
        await definitionCache.ClearAsync();

        // Touch the type initializer (if any) on the test ExecutionContext first, mirroring
        // the production scenario where startup pre-warms it on a different ExecutionContext.
        _ = await _store.GetOrNullAsync("FeatureGatedPermission");

        await groupCache.ClearAsync();
        await definitionCache.ClearAsync();

        PermissionDefinition permission = null;
        Task task;
        using (ExecutionContext.SuppressFlow())
        {
            task = Task.Run(async () =>
            {
                permission = await _store.GetOrNullAsync("FeatureGatedPermission");
            });
        }
        await task;

        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("FeatureGatedPermission");
    }
}
