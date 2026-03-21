using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class DynamicPermissionDefinitionStoreInMemoryCache_Tests : PermissionTestBase
{
    private readonly IDynamicPermissionDefinitionStoreInMemoryCache _cache;

    public DynamicPermissionDefinitionStoreInMemoryCache_Tests()
    {
        _cache = GetRequiredService<IDynamicPermissionDefinitionStoreInMemoryCache>();
    }

    [Fact]
    public async Task FillAsync_Should_Populate_ResourcePermissionDefinitions()
    {
        // Arrange
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>();
        var permissionRecords = new List<PermissionDefinitionRecord>
        {
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: null,
                name: "TestResourcePerm1",
                resourceName: "TestResource",
                managementPermissionName: "TestManagementPerm",
                parentName: null,
                displayName: "F:Test Resource Permission 1",
                isEnabled: true,
                multiTenancySide: MultiTenancySides.Both,
                providers: "R,U",
                stateCheckers: null
            )
        };

        // Act
        await _cache.FillAsync(permissionGroupRecords, permissionRecords);

        // Assert
        var resourcePermissions = _cache.GetResourcePermissions();
        resourcePermissions.Count.ShouldBe(1);

        var resourcePermission = _cache.GetResourcePermissionOrNull("TestResource", "TestResourcePerm1");
        resourcePermission.ShouldNotBeNull();
        resourcePermission.Name.ShouldBe("TestResourcePerm1");
        resourcePermission.ResourceName.ShouldBe("TestResource");
        resourcePermission.ManagementPermissionName.ShouldBe("TestManagementPerm");
        resourcePermission.IsEnabled.ShouldBeTrue();
        resourcePermission.MultiTenancySide.ShouldBe(MultiTenancySides.Both);
        resourcePermission.Providers.Count.ShouldBe(2);
        resourcePermission.Providers.ShouldContain("R");
        resourcePermission.Providers.ShouldContain("U");
    }

    [Fact]
    public async Task FillAsync_Should_Populate_ResourcePermission_With_ExtraProperties()
    {
        // Arrange
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>();
        var record = new PermissionDefinitionRecord(
            Guid.NewGuid(),
            groupName: null,
            name: "TestResourcePerm2",
            resourceName: "TestResource",
            managementPermissionName: "TestManagementPerm",
            parentName: null,
            displayName: "F:Test Resource Permission 2"
        );
        record.ExtraProperties["CustomProp1"] = "CustomValue1";

        var permissionRecords = new List<PermissionDefinitionRecord> { record };

        // Act
        await _cache.FillAsync(permissionGroupRecords, permissionRecords);

        // Assert
        var resourcePermission = _cache.GetResourcePermissionOrNull("TestResource", "TestResourcePerm2");
        resourcePermission.ShouldNotBeNull();
        resourcePermission["CustomProp1"].ShouldBe("CustomValue1");
    }

    [Fact]
    public async Task FillAsync_Should_Not_Mix_Resource_And_Regular_Permissions()
    {
        // Arrange
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>
        {
            new PermissionGroupDefinitionRecord(
                Guid.NewGuid(),
                name: "TestGroup",
                displayName: "F:Test Group"
            )
        };

        var permissionRecords = new List<PermissionDefinitionRecord>
        {
            // Regular permission
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: "TestGroup",
                name: "RegularPerm1",
                resourceName: null,
                managementPermissionName: null,
                parentName: null,
                displayName: "F:Regular Permission 1"
            ),
            // Resource permission
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: null,
                name: "ResourcePerm1",
                resourceName: "TestResource",
                managementPermissionName: "ManagementPerm",
                parentName: null,
                displayName: "F:Resource Permission 1"
            )
        };

        // Act
        await _cache.FillAsync(permissionGroupRecords, permissionRecords);

        // Assert
        var regularPermissions = _cache.GetPermissions();
        regularPermissions.Count.ShouldBe(1);
        regularPermissions.First().Name.ShouldBe("RegularPerm1");

        var resourcePermissions = _cache.GetResourcePermissions();
        resourcePermissions.Count.ShouldBe(1);
        resourcePermissions.First().Name.ShouldBe("ResourcePerm1");

        _cache.GetPermissionOrNull("RegularPerm1").ShouldNotBeNull();
        _cache.GetPermissionOrNull("ResourcePerm1").ShouldBeNull();

        _cache.GetResourcePermissionOrNull("TestResource", "ResourcePerm1").ShouldNotBeNull();
        _cache.GetResourcePermissionOrNull("TestResource", "RegularPerm1").ShouldBeNull();
    }

    [Fact]
    public async Task FillAsync_Should_Populate_ResourcePermission_With_StateCheckers()
    {
        // Arrange
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>();
        var permissionRecords = new List<PermissionDefinitionRecord>
        {
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: null,
                name: "TestResourcePerm3",
                resourceName: "TestResource",
                managementPermissionName: "TestManagementPerm",
                parentName: null,
                displayName: "F:Test Resource Permission 3",
                stateCheckers: "[{\"T\":\"A\"}]"
            )
        };

        // Act
        await _cache.FillAsync(permissionGroupRecords, permissionRecords);

        // Assert
        var resourcePermission = _cache.GetResourcePermissionOrNull("TestResource", "TestResourcePerm3");
        resourcePermission.ShouldNotBeNull();
        resourcePermission.StateCheckers.Count.ShouldBe(1);
    }

    [Fact]
    public async Task FillAsync_Should_Clear_Previous_ResourcePermissions()
    {
        // Arrange - first fill
        var permissionRecords1 = new List<PermissionDefinitionRecord>
        {
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: null,
                name: "OldResourcePerm",
                resourceName: "TestResource",
                managementPermissionName: "ManagementPerm",
                parentName: null,
                displayName: "F:Old Resource Permission"
            )
        };
        await _cache.FillAsync(new List<PermissionGroupDefinitionRecord>(), permissionRecords1);
        _cache.GetResourcePermissions().Count.ShouldBe(1);

        // Arrange - second fill with different data
        var permissionRecords2 = new List<PermissionDefinitionRecord>
        {
            new PermissionDefinitionRecord(
                Guid.NewGuid(),
                groupName: null,
                name: "NewResourcePerm",
                resourceName: "TestResource",
                managementPermissionName: "ManagementPerm",
                parentName: null,
                displayName: "F:New Resource Permission"
            )
        };

        // Act
        await _cache.FillAsync(new List<PermissionGroupDefinitionRecord>(), permissionRecords2);

        // Assert
        var resourcePermissions = _cache.GetResourcePermissions();
        resourcePermissions.Count.ShouldBe(1);
        resourcePermissions.First().Name.ShouldBe("NewResourcePerm");
        _cache.GetResourcePermissionOrNull("TestResource", "OldResourcePerm").ShouldBeNull();
    }
}
