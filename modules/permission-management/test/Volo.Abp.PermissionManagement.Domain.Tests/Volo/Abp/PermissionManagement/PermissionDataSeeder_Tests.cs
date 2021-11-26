using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionDataSeeder_Tests : PermissionTestBase
{
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IPermissionGrantRepository _grantpermissionGrantRepository;
    private readonly ICurrentTenant _currentTenant;

    public PermissionDataSeeder_Tests()
    {
        _permissionDataSeeder = GetRequiredService<IPermissionDataSeeder>();
        _grantpermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task SeedAsync()
    {
        (await _grantpermissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
        (await _grantpermissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldBeNull();

        await _permissionDataSeeder.SeedAsync("Test", "Test", new List<string>()
            {
                "MyPermission1",
                "MyPermission2"
            });

        (await _grantpermissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();
        (await _grantpermissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Seed_With_TenantId()
    {
        //Seed without tenant
        await _permissionDataSeeder.SeedAsync("Test", "Test", new List<string>()
            {
                "MyPermission1",
                "MyPermission2"
            });

        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            (await _grantpermissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();
            (await _grantpermissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldBeNull();
        }

        await _permissionDataSeeder.SeedAsync("Test", "Test", new List<string>()
            {
                "MyPermission1",
                "MyPermission2"
            }, tenantId);

        using (_currentTenant.Change(tenantId))
        {
            (await _grantpermissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();
            (await _grantpermissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
        }
    }
}
