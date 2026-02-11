using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TenantManagement;

public class TenantValidator_Tests : AbpTenantManagementDomainTestBase
{
    private readonly TenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;

    public TenantValidator_Tests()
    {
        _tenantManager = GetRequiredService<TenantManager>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
    }

    [Fact]
    public async Task Should_Throw_If_Name_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _tenantManager.CreateAsync(""));
    }

    [Fact]
    public async Task Should_Throw_If_Duplicate_Name()
    {
        await Assert.ThrowsAsync<BusinessException>(() => _tenantManager.CreateAsync("VOLOSOFT"));

        var tenant = await _tenantRepository.FindByNameAsync("ABP");
        await Assert.ThrowsAsync<BusinessException>(() => _tenantManager.ChangeNameAsync(tenant, "VOLOSOFT"));
    }

    [Fact]
    public async Task Should_Not_Throw_For_Unique_Name()
    {
        var tenant = await _tenantManager.CreateAsync("VOLOSOFT2");
        await _tenantRepository.InsertAsync(tenant);

        tenant = await _tenantRepository.FindByNameAsync("ABP");
        await _tenantManager.ChangeNameAsync(tenant, "VOLOSOFT3");
        await _tenantRepository.UpdateAsync(tenant);

        tenant = await _tenantRepository.FindByNameAsync("VOLOSOFT3");
        tenant.ShouldNotBeNull();
    }
}
