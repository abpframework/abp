using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionManager_Tests : PermissionTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
    }

    [Fact]
    public async Task GetAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        var grantedProviders = await _permissionManager.GetAsync("MyPermission1",
            "Test",
            "Test");

        grantedProviders.ShouldNotBeNull();
        grantedProviders.IsGranted.ShouldBeTrue();
        grantedProviders.Name.ShouldBe("MyPermission1");
        grantedProviders.Providers.ShouldContain(x => x.Key == "Test");
    }

    [Fact]
    public async Task Multiple_GetAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission2",
            "Test",
            "Test")
        );

        var grantedProviders = await _permissionManager.GetAsync(
            new[] { "MyPermission1", "MyPermission2" },
            "Test",
            "Test");

        grantedProviders.Result.Count.ShouldBe(2);
        grantedProviders.Result.First().IsGranted.ShouldBeTrue();
        grantedProviders.Result.First().Name.ShouldBe("MyPermission1");
        grantedProviders.Result.First().Providers.ShouldContain(x => x.Key == "Test");

        grantedProviders.Result.Last().IsGranted.ShouldBeTrue();
        grantedProviders.Result.Last().Name.ShouldBe("MyPermission2");
        grantedProviders.Result.Last().Providers.ShouldContain(x => x.Key == "Test");
    }

    [Fact]
    public async Task Get_Should_Exception_When_Permission_Undefined()
    {
        await Assert.ThrowsAsync<AbpException>(async () => await _permissionManager.GetAsync(
            "MyPermission1NotExist",
            "Test",
            "Test"));
    }

    [Fact]
    public async Task GetAllAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission2",
            "Test",
            "Test")
        );

        var permissionWithGrantedProviders = await _permissionManager.GetAllAsync(
            "Test",
            "Test");

        permissionWithGrantedProviders.ShouldNotBeNull();
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyPermission1" && x.Providers.Any(p => p.Key == "Test"));
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyPermission2" && x.Providers.Any(p => p.Key == "Test"));
    }

    [Fact]
    public async Task SetAsync()
    {
        (await _permissionGrantRepository.FindAsync("MyPermission2",
            "Test",
            "Test")).ShouldBeNull();

        await _permissionManager.SetAsync(
            "MyPermission2",
            "Test",
            "Test", true);

        (await _permissionGrantRepository.FindAsync("MyPermission2",
            "Test",
            "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Set_Should_Exception_When_Permission_Undefined()
    {
        await Assert.ThrowsAsync<AbpException>(async () => await _permissionManager.SetAsync(
            "MyPermission1NotExist",
            "Test",
            "Test",
            true));
    }

    [Fact]
    public async Task UpdateProviderKey()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );
        var permissionGrant = await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test");
        permissionGrant.ProviderKey.ShouldBe("Test");

        await _permissionManager.UpdateProviderKeyAsync(permissionGrant, "NewProviderKey");
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "NewProviderKey")).ShouldNotBeNull();
    }
}
