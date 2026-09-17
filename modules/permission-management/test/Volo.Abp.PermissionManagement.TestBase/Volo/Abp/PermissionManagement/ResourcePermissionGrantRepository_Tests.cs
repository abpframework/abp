using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public abstract class ResourcePermissionGrantRepository_Tests<TStartupModule> : PermissionManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    protected ResourcePermissionGrantRepository_Tests()
    {
        ResourcePermissionGrantRepository = GetRequiredService<IResourcePermissionGrantRepository>();
    }

    [Fact]
    public async Task FindAsync()
    {
        (await ResourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldNotBeNull();

        (await ResourcePermissionGrantRepository.FindAsync(
            "Undefined-Permission",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldBeNull();

        (await ResourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Undefined-Provider",
            "Unknown-Id")).ShouldBeNull();
    }

    [Fact]
    public async Task GetList4Async()
    {
        var permissionGrants =
            await ResourcePermissionGrantRepository.GetListAsync(
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey3,
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission3");
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission5");
    }

    [Fact]
    public async Task GetList5Async()
    {
        var permissionGrants =
            await ResourcePermissionGrantRepository.GetListAsync(
                new[] { "MyResourcePermission1", "MyResourcePermission3", "MyResourcePermission5" },
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey3,
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

        permissionGrants.ShouldNotContain(p => p.Name == "MyResourcePermission1");
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission3");
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission5");
    }

    [Fact]
    public async Task GetList2Async()
    {
        var permissionGrants =
            await ResourcePermissionGrantRepository.GetListAsync(
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission1" && p.ResourceKey == TestEntityResource.ResourceKey1 && p.ResourceName == TestEntityResource.ResourceName);
        permissionGrants.ShouldContain(p => p.Name == "MyDisabledResourcePermission1" && p.ResourceKey == TestEntityResource.ResourceKey1 && p.ResourceName == TestEntityResource.ResourceName);
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission3" && p.ResourceKey == TestEntityResource.ResourceKey3 && p.ResourceName == TestEntityResource.ResourceName);
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission5" && p.ResourceKey == TestEntityResource.ResourceKey3 && p.ResourceName == TestEntityResource.ResourceName);
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission5" && p.ResourceKey == TestEntityResource.ResourceKey5 && p.ResourceName == TestEntityResource.ResourceName);

        permissionGrants =
            await ResourcePermissionGrantRepository.GetListAsync(
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User2Id.ToString());

        permissionGrants.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetPermissionsAsync()
    {
        var permissionGrants =
            await ResourcePermissionGrantRepository.GetPermissionsAsync(
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1);

        permissionGrants.Count.ShouldBe(2);
        permissionGrants.ShouldContain(p => p.Name == "MyResourcePermission1");
        permissionGrants.ShouldContain(p => p.Name == "MyDisabledResourcePermission1");
    }

    [Fact]
    public async Task GetResourceKeys()
    {
        var permissionGrants =
            await ResourcePermissionGrantRepository.GetResourceKeys(
                TestEntityResource.ResourceName,
                "MyResourcePermission5");

        permissionGrants.Count.ShouldBe(2);
        permissionGrants.ShouldContain(p => p.ResourceKey == TestEntityResource.ResourceKey3);
        permissionGrants.ShouldContain(p => p.ResourceKey == TestEntityResource.ResourceKey5);
    }
}
