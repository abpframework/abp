using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.TenantManagement;

public class TenantManager_Tests : AbpTenantManagementDomainTestBase
{
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantNormalizer _tenantNormalizer;

    public TenantManager_Tests()
    {
        _tenantManager = GetRequiredService<ITenantManager>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var tenant = await _tenantManager.CreateAsync("Test");
        tenant.Name.ShouldBe("Test");
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("Test"));
    }

    [Fact]
    public async Task Create_Tenant_Name_Can_Not_Duplicate()
    {
        await Assert.ThrowsAsync<BusinessException>(async () => await _tenantManager.CreateAsync("volosoft"));
    }

    [Fact]
    public async Task ChangeNameAsync()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("volosoft"));
        tenant.ShouldNotBeNull();
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("volosoft"));

        await _tenantManager.ChangeNameAsync(tenant, "newVolosoft");

        tenant.Name.ShouldBe("newVolosoft");
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("newVolosoft"));
    }

    [Fact]
    public async Task ChangeName_Tenant_Name_Can_Not_Duplicate()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme"));
        tenant.ShouldNotBeNull();

        await Assert.ThrowsAsync<BusinessException>(async () => await _tenantManager.ChangeNameAsync(tenant, "volosoft"));
    }
}
