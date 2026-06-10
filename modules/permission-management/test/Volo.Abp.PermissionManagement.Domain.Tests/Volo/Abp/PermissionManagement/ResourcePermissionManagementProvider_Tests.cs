using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionManagementProvider_Tests : PermissionTestBase
{
    private readonly IResourcePermissionManagementProvider _resourcePermissionManagementProvider;
    private readonly IResourcePermissionGrantRepository _resourcePermissionGrantRepository;

    public ResourcePermissionManagementProvider_Tests()
    {
        _resourcePermissionManagementProvider = GetRequiredService<TestResourcePermissionManagementProvider>();
        _resourcePermissionGrantRepository = GetRequiredService<IResourcePermissionGrantRepository>();
    }

    [Fact]
    public async Task CheckAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                Guid.NewGuid(),
                "MyPermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "Test",
                "Test"
            )
        );

        var permissionValueProviderGrantInfo = await _resourcePermissionManagementProvider.CheckAsync(
            "MyPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        permissionValueProviderGrantInfo.IsGranted.ShouldBeTrue();
        permissionValueProviderGrantInfo.ProviderKey.ShouldBe("Test");
    }

    [Fact]
    public async Task Check_Should_Return_NonGranted_When_ProviderName_NotEquals_Name()
    {
        var permissionValueProviderGrantInfo = await _resourcePermissionManagementProvider.CheckAsync(
            "MyPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "TestNotExist",
            "Test");

        permissionValueProviderGrantInfo.IsGranted.ShouldBeFalse();
        permissionValueProviderGrantInfo.ProviderKey.ShouldBeNull();
    }

    [Fact]
    public async Task SetAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(
            new ResourcePermissionGrant(
                Guid.NewGuid(),
                "MyPermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "Test",
                "Test"
            )
        );
        (await _resourcePermissionGrantRepository.FindAsync("MyPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")).ShouldNotBeNull();

        await _resourcePermissionManagementProvider.SetAsync("MyPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            false);

        (await _resourcePermissionGrantRepository.FindAsync("MyPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")).ShouldBeNull();
    }
}
