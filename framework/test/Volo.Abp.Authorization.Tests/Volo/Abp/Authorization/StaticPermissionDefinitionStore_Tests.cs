using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
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
}
