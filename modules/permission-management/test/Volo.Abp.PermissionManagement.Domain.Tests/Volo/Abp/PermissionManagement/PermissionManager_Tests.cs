using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionManager_Tests : PermissionTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly TestGlobalPermissionStateCheckerCounter _stateCheckerCounter;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        _stateCheckerCounter = GetRequiredService<TestGlobalPermissionStateCheckerCounter>();
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
    public async Task Multiple_Get_Should_Apply_State_Checkers_Per_Permission()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission5",
            "Test",
            "Test")
        );

        var names = new[] { "MyPermission1", "MyPermission5" };

        _stateCheckerCounter.Reset();
        var grantedProviders = await _permissionManager.GetAsync(names, "Test", "Test");
        _stateCheckerCounter.BatchCheckCount.ShouldBe(1);
        _stateCheckerCounter.SingleCheckCount.ShouldBe(0);

        grantedProviders.Result.Single(x => x.Name == "MyPermission1").IsGranted.ShouldBeTrue();
        grantedProviders.Result.Single(x => x.Name == "MyPermission5").IsGranted.ShouldBeFalse();

        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "super-admin")))
        {
            grantedProviders = await _permissionManager.GetAsync(names, "Test", "Test");

            grantedProviders.Result.Single(x => x.Name == "MyPermission1").IsGranted.ShouldBeTrue();
            grantedProviders.Result.Single(x => x.Name == "MyPermission5").IsGranted.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Multiple_Get_Should_Return_Not_Granted_When_Every_Permission_Is_Filtered_Out()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyDisabledPermission1",
            "Test",
            "Test")
        );

        _stateCheckerCounter.Reset();
        var grantedProviders = await _permissionManager.GetAsync(
            new[] { "MyDisabledPermission1", "MyPermission1NotExist" },
            "Test",
            "Test");

        grantedProviders.Result.Count.ShouldBe(2);
        grantedProviders.Result.ShouldAllBe(x => !x.IsGranted);
        _stateCheckerCounter.BatchCheckCount.ShouldBe(0);
        _stateCheckerCounter.SingleCheckCount.ShouldBe(0);
    }

    [Fact]
    public async Task Get_Should_Return_Not_Granted_When_Permission_Undefined()
    {
        var result = await _permissionManager.GetAsync("MyPermission1NotExist", "Test", "Test");
        result.Name.ShouldBe("MyPermission1NotExist");
        result.Providers.ShouldBeEmpty();
        result.IsGranted.ShouldBeFalse();
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
    public async Task Set_Should_Silently_Ignore_When_Permission_Undefined()
    {
        await _permissionManager.SetAsync(
            "MyPermission1NotExist",
            "Test",
            "Test",
            true);
    }

    [Fact]
    public async Task Set_Should_Throw_Exception_If_Provider_Not_Found()
    {
       var exception =  await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _permissionManager.SetAsync(
                "MyPermission1",
                "UndefinedProvider",
                "Test",
                true);
        });

        exception.Message.ShouldBe("Unknown permission management provider: UndefinedProvider");
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

    [Fact]
    public async Task DeleteAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );
        var permissionGrant = await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test");
        permissionGrant.ProviderKey.ShouldBe("Test");

        await _permissionManager.DeleteAsync("Test","Test");
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
    }
}
