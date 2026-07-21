using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionManager_Tests : PermissionTestBase
{
    private readonly IResourcePermissionManager _resourcePermissionManager;
    private readonly IResourcePermissionGrantRepository _resourcePermissionGrantRepository;

    public ResourcePermissionManager_Tests()
    {
        _resourcePermissionManager = GetRequiredService<IResourcePermissionManager>();
        _resourcePermissionGrantRepository = GetRequiredService<IResourcePermissionGrantRepository>();
    }

    [Fact]
    public async Task GetProviderKeyLookupServicesAsync()
    {
        var permissionProviderKeyLookupServices = await _resourcePermissionManager.GetProviderKeyLookupServicesAsync();

        permissionProviderKeyLookupServices.ShouldNotBeNull();
        permissionProviderKeyLookupServices.First().Name.ShouldBe("Test");
    }

    [Fact]
    public async Task GetProviderKeyLookupServiceAsync()
    {
        var testProviderKeyLookupService = await _resourcePermissionManager.GetProviderKeyLookupServiceAsync("Test");
        testProviderKeyLookupService.ShouldNotBeNull();
        testProviderKeyLookupService.Name.ShouldBe("Test");

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.GetProviderKeyLookupServiceAsync("UndefinedProvider");
        });
        exception.Message.ShouldBe("Unknown resource permission provider key lookup service: UndefinedProvider");

        var unavailableException = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.GetProviderKeyLookupServiceAsync("TestUnavailable");
        });
        unavailableException.Message.ShouldBe("The resource permission provider key lookup service 'TestUnavailable' is not available in the current context.");
    }

    [Fact]
    public async Task GetAvailablePermissionsAsync()
    {
        var availablePermissions = await _resourcePermissionManager.GetAvailablePermissionsAsync(TestEntityResource.ResourceName);

        availablePermissions.ShouldNotBeNull();
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission1");
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission2");
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission3");
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission4");
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission6");
        availablePermissions.ShouldContain(p => p.Name == "MyResourcePermission7");

        availablePermissions.ShouldNotContain(p => p.Name == "MyResourcePermission5");
        availablePermissions.ShouldNotContain(p => p.Name == "MyResourceDisabledPermission1");
        availablePermissions.ShouldNotContain(p => p.Name == "MyResourceDisabledPermission2");
    }

    [Fact]
    public async Task GetAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );

        var grantedProviders = await _resourcePermissionManager.GetAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        grantedProviders.ShouldNotBeNull();
        grantedProviders.IsGranted.ShouldBeTrue();
        grantedProviders.Name.ShouldBe("MyResourcePermission1");
        grantedProviders.Providers.ShouldContain(x => x.Key == "Test");
    }

    [Fact]
    public async Task Multiple_GetAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission2",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );

        var grantedProviders = await _resourcePermissionManager.GetAsync(
            new[] { "MyResourcePermission1", "MyResourcePermission2" },
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        grantedProviders.Result.Count.ShouldBe(2);
        grantedProviders.Result.First().IsGranted.ShouldBeTrue();
        grantedProviders.Result.First().Name.ShouldBe("MyResourcePermission1");
        grantedProviders.Result.First().Providers.ShouldContain(x => x.Key == "Test");

        grantedProviders.Result.Last().IsGranted.ShouldBeTrue();
        grantedProviders.Result.Last().Name.ShouldBe("MyResourcePermission2");
        grantedProviders.Result.Last().Providers.ShouldContain(x => x.Key == "Test");
    }

    [Fact]
    public async Task Get_Should_Return_Not_Granted_When_Permission_Undefined()
    {
        var result = await _resourcePermissionManager.GetAsync(
            "MyResourcePermission1NotExist",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,"Test", "Test");
        result.Name.ShouldBe("MyResourcePermission1NotExist");
        result.Providers.ShouldBeEmpty();
        result.IsGranted.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );

        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission2",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );

        var permissionWithGrantedProviders = await _resourcePermissionManager.GetAllAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        permissionWithGrantedProviders.ShouldNotBeNull();
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyResourcePermission1" && x.Providers.Any(p => p.Key == "Test"));
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyResourcePermission2" && x.Providers.Any(p => p.Key == "Test"));


        permissionWithGrantedProviders = await _resourcePermissionManager.GetAllAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        permissionWithGrantedProviders.ShouldNotBeNull();
        permissionWithGrantedProviders.ShouldContain(x => x.IsGranted && x.Name == "MyResourcePermission1" && x.Providers.Any(p => p.Key == "Test"));
        permissionWithGrantedProviders.ShouldContain(x => x.IsGranted && x.Name == "MyResourcePermission2" && x.Providers.Any(p => p.Key == "Test"));

        permissionWithGrantedProviders.ShouldNotContain(x => x.Name == "MyResourcePermission5"); // Not available permission

        permissionWithGrantedProviders.ShouldContain(x => !x.IsGranted && x.Name == "MyResourcePermission3" && x.Providers.Count == 0);
        permissionWithGrantedProviders.ShouldContain(x => !x.IsGranted && x.Name == "MyResourcePermission4" && x.Providers.Count == 0);
        permissionWithGrantedProviders.ShouldContain(x => !x.IsGranted && x.Name == "MyResourcePermission6" && x.Providers.Count == 0);
        permissionWithGrantedProviders.ShouldContain(x => !x.IsGranted && x.Name == "MyResourcePermission7" && x.Providers.Count == 0);
        permissionWithGrantedProviders.ShouldContain(x => !x.IsGranted && x.Name == "MyResourcePermission8" && x.Providers.Count == 0);
    }

    [Fact]
    public async Task GetAllGroupAsync()
    {
        var group = await _resourcePermissionManager.GetAllGroupAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        group.ShouldNotBeNull();
        group.Count.ShouldBe(1);
        group.First().ProviderName.ShouldBe(UserResourcePermissionValueProvider.ProviderName);
        group.First().ProviderKey.ShouldBe(PermissionTestDataBuilder.User1Id.ToString());
        group.First().Permissions.Count.ShouldBe(1);
        group.First().Permissions.ShouldContain(x => x == "MyResourcePermission1");

        group = await _resourcePermissionManager.GetAllGroupAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey3);

        group.ShouldNotBeNull();
        group.Count.ShouldBe(1);
        group.First().ProviderName.ShouldBe(UserResourcePermissionValueProvider.ProviderName);
        group.First().ProviderKey.ShouldBe(PermissionTestDataBuilder.User1Id.ToString());
        group.First().Permissions.Count.ShouldBe(2);
        group.First().Permissions.ShouldContain(x => x == "MyResourcePermission3");
        group.First().Permissions.ShouldContain(x => x == "MyResourcePermission6");
    }

    [Fact]
    public async Task Set_Should_Silently_Ignore_When_Permission_Undefined()
    {
        await _resourcePermissionManager.SetAsync(
            "MyResourcePermission1NotExist",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test",
            true);
    }

    [Fact]
    public async Task Set_Should_Throw_Exception_If_Provider_Not_Found()
    {
       var exception =  await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.SetAsync(
                "MyResourcePermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "UndefinedProvider",
                "Test",
                true);
        });

        exception.Message.ShouldBe("Unknown resource permission management provider: UndefinedProvider");
    }

    [Fact]
    public async Task UpdateProviderKey()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );
        var permissionGrant = await _resourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");
        permissionGrant.ProviderKey.ShouldBe("Test");

        await _resourcePermissionManager.UpdateProviderKeyAsync(permissionGrant, "NewProviderKey");
        (await _resourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "NewProviderKey")).ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );
        var permissionGrant = await _resourcePermissionGrantRepository.FindAsync("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");
        permissionGrant.ProviderKey.ShouldBe("Test");

        await _resourcePermissionManager.DeleteAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        (await _resourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task DeleteByProviderAsync()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")
        );

        var permissionGrant = await _resourcePermissionGrantRepository.FindAsync("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test");

        permissionGrant.ProviderKey.ShouldBe("Test");

        await _resourcePermissionManager.DeleteAsync(
            "Test",
            "Test");

        (await _resourcePermissionGrantRepository.FindAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task GetProviderKeyLookupServicesAsync_Should_Not_Return_Unavailable_Services()
    {
        var lookupServices = await _resourcePermissionManager.GetProviderKeyLookupServicesAsync();

        lookupServices.ShouldContain(s => s.Name == "Test");
        lookupServices.ShouldNotContain(s => s.Name == "TestUnavailable");
    }

    [Fact]
    public async Task GetAsync_Should_Not_Return_Grant_From_Unavailable_Provider()
    {
        // Insert a grant directly via repository to simulate data stored by an unavailable provider
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "TestUnavailable",
            "someKey")
        );

        var grantedProviders = await _resourcePermissionManager.GetAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "TestUnavailable",
            "someKey");

        // The unavailable provider is skipped, so the permission should not be considered granted via it
        grantedProviders.IsGranted.ShouldBeFalse();
        grantedProviders.Providers.ShouldNotContain(p => p.Name == "TestUnavailable");
    }

    [Fact]
    public async Task SetAsync_Should_Throw_When_Provider_Is_Unavailable()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.SetAsync(
                "MyResourcePermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "TestUnavailable",
                "someKey",
                true);
        });

        exception.Message.ShouldBe("The resource permission management provider 'TestUnavailable' is not available in the current context.");
    }

    [Fact]
    public async Task GetAllAsync_Should_Not_Include_Grants_From_Unavailable_Provider()
    {
        // Grant via the available "Test" provider and the unavailable "TestUnavailable" provider
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "Test",
            "someKey")
        );
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "TestUnavailable",
            "someKey")
        );

        var result = await _resourcePermissionManager.GetAllAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        var item = result.FirstOrDefault(x => x.Name == "MyResourcePermission1");
        item.ShouldNotBeNull();
        item.IsGranted.ShouldBeTrue();
        item.Providers.ShouldContain(p => p.Name == "Test");
        item.Providers.ShouldNotContain(p => p.Name == "TestUnavailable");
    }

    [Fact]
    public async Task GetAllGroupAsync_Should_Not_Include_Grants_From_Unavailable_Provider()
    {
        await _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            "MyResourcePermission2",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            "TestUnavailable",
            "someKey")
        );

        var group = await _resourcePermissionManager.GetAllGroupAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        group.ShouldNotContain(g => g.ProviderName == "TestUnavailable");
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Provider_Is_Unavailable()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.DeleteAsync(
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "TestUnavailable",
                "someKey");
        });
        exception.Message.ShouldBe("The resource permission management provider 'TestUnavailable' is not available in the current context.");
    }

    [Fact]
    public async Task DeleteAsyncByName_Should_Throw_When_Provider_Is_Unavailable()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _resourcePermissionManager.DeleteAsync(
                "MyResourcePermission1",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey1,
                "TestUnavailable",
                "someKey");
        });
        exception.Message.ShouldBe("The resource permission management provider 'TestUnavailable' is not available in the current context.");
    }
}
