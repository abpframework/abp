using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionManager_Tests : PermissionTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly TestPermissionManagementProvider _testPermissionManagementProvider;
    private readonly ICurrentTenant _currentTenant;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _testPermissionManagementProvider = GetRequiredService<TestPermissionManagementProvider>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
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
    public async Task Multiple_SetAsync()
    {
        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", true),
                new KeyValuePair<string, bool>("MyPermission2", true)
            },
            "Test",
            "Test");

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Grant_And_Revoke_Together()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", false),
                new KeyValuePair<string, bool>("MyPermission2", true)
            },
            "Test",
            "Test");

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Do_Nothing_When_No_Permission_Is_Passed()
    {
        await _permissionManager.SetAsync(
            Array.Empty<KeyValuePair<string, bool>>(),
            "Test",
            "Test");

        _testPermissionManagementProvider.CheckCalls.ShouldBeEmpty();
        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Query_The_Current_State_Only_Once()
    {
        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", true),
                new KeyValuePair<string, bool>("MyPermission2", true),
                new KeyValuePair<string, bool>("MyPermission7", true),
                new KeyValuePair<string, bool>("MyPermission8", true)
            },
            "Test",
            "Test");

        _testPermissionManagementProvider.CheckCalls.ShouldHaveSingleItem();
        _testPermissionManagementProvider.CheckCalls.Single().ShouldBe(
            new[] { "MyPermission1", "MyPermission2", "MyPermission7", "MyPermission8" });
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Throw_Exception_If_A_Permission_Is_Not_Compatible_With_The_Multi_Tenancy_Side()
    {
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _permissionManager.SetAsync(
                    new[]
                    {
                        new KeyValuePair<string, bool>("MyPermission1", true),
                        new KeyValuePair<string, bool>("MyPermission3", true)
                    },
                    "Test",
                    "Test");
            });

            exception.Message.ShouldContain("MyPermission3");
            exception.Message.ShouldContain("multitenancy side");
            _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
        }
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Not_Write_Unchanged_Permissions()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", true),
                new KeyValuePair<string, bool>("MyPermission2", false)
            },
            "Test",
            "Test");

        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Only_Write_Changed_Permissions()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", true),
                new KeyValuePair<string, bool>("MyPermission2", true)
            },
            "Test",
            "Test");

        _testPermissionManagementProvider.SetCalls.ShouldHaveSingleItem();
        _testPermissionManagementProvider.SetCalls.Single().Key.ShouldBe("MyPermission2");
        _testPermissionManagementProvider.SetCalls.Single().Value.ShouldBeTrue();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Use_The_Last_State_Of_A_Repeated_Permission()
    {
        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1", true),
                new KeyValuePair<string, bool>("MyPermission1", false)
            },
            "Test",
            "Test");

        /* The last state is "not granted", which is also the current state, so nothing is written. */
        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Throw_Exception_If_A_Permission_Is_Disabled_By_A_State_Checker()
    {
        var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
        {
            await _permissionManager.SetAsync(
                new[]
                {
                    new KeyValuePair<string, bool>("MyPermission1", true),
                    new KeyValuePair<string, bool>("MyPermission5", true)
                },
                "Test",
                "Test");
        });

        exception.Message.ShouldBe("The permission named 'MyPermission5' is disabled!");
        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Throw_Exception_If_A_Permission_Is_Not_Compatible_With_The_Provider()
    {
        var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
        {
            await _permissionManager.SetAsync(
                new[]
                {
                    new KeyValuePair<string, bool>("MyPermission1", true),
                    new KeyValuePair<string, bool>("MyPermission4", true)
                },
                "Test",
                "Test");
        });

        exception.Message.ShouldBe("The permission named 'MyPermission4' is not compatible with the provider named 'Test'");
        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Multiple_SetAsync_Should_Not_Write_Anything_When_A_Permission_Is_Disabled()
    {
        var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
        {
            await _permissionManager.SetAsync(
                new[]
                {
                    new KeyValuePair<string, bool>("MyPermission1", true),
                    new KeyValuePair<string, bool>("MyDisabledPermission1", true)
                },
                "Test",
                "Test");
        });

        exception.Message.ShouldBe("The permission named 'MyDisabledPermission1' is disabled!");
        _testPermissionManagementProvider.SetCalls.ShouldBeEmpty();
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task Multiple_Set_Should_Silently_Ignore_When_Permission_Undefined()
    {
        await _permissionManager.SetAsync(
            new[]
            {
                new KeyValuePair<string, bool>("MyPermission1NotExist", true),
                new KeyValuePair<string, bool>("MyPermission1", true)
            },
            "Test",
            "Test");

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Multiple_Set_Should_Throw_Exception_If_Provider_Not_Found()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _permissionManager.SetAsync(
                new[] { new KeyValuePair<string, bool>("MyPermission1", true) },
                "UndefinedProvider",
                "Test");
        });

        exception.Message.ShouldBe("Unknown permission management provider: UndefinedProvider");
    }

    [Fact]
    public async Task Multiple_Set_Should_Not_Throw_Exception_If_Provider_Not_Found_But_Nothing_Changed()
    {
        await _permissionManager.SetAsync(
            new[] { new KeyValuePair<string, bool>("MyPermission1", false) },
            "UndefinedProvider",
            "Test");
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
