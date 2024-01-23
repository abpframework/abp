using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.TenantManagement;

public class TenantConfigurationCacheItemInvalidator_Tests : AbpTenantManagementDomainTestBase
{
    private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;
    private readonly ITenantStore _tenantStore;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantManager _tenantManager;

    public TenantConfigurationCacheItemInvalidator_Tests()
    {
        _cache = GetRequiredService<IDistributedCache<TenantConfigurationCacheItem>>();
        _tenantStore = GetRequiredService<ITenantStore>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantManager = GetRequiredService<ITenantManager>();
    }

    [Fact]
    public async Task Get_Tenant_Should_Cached()
    {
        var acme = await _tenantRepository.FindByNameAsync("acme");
        acme.ShouldNotBeNull();

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.Name))).ShouldBeNull();

        await _tenantStore.FindAsync(acme.Id);
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();

        await _tenantStore.FindAsync(acme.Name);
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.Name))).ShouldNotBeNull();


        var volosoft = _tenantRepository.FindByName("volosoft");
        volosoft.ShouldNotBeNull();

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(volosoft.Id, null))).ShouldBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, volosoft.Name))).ShouldBeNull();

        _tenantStore.Find(volosoft.Id);
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(volosoft.Id, null))).ShouldNotBeNull();

        _tenantStore.Find(volosoft.Name);
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, volosoft.Name))).ShouldNotBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_When_Tenant_Changed()
    {
        var acme = await _tenantRepository.FindByNameAsync("acme");
        acme.ShouldNotBeNull();

        // FindAsync will cache tenant.
        await _tenantStore.FindAsync(acme.Id);
        await _tenantStore.FindAsync(acme.Name);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.Name))).ShouldNotBeNull();

        await _tenantRepository.DeleteAsync(acme);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.Name))).ShouldBeNull();


        var volosoft = await _tenantRepository.FindByNameAsync("volosoft");
        volosoft.ShouldNotBeNull();

        // Find will cache tenant.
        _tenantStore.Find(volosoft.Id);
        _tenantStore.Find(volosoft.Name);

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(volosoft.Id, null))).ShouldNotBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, volosoft.Name))).ShouldNotBeNull();

        await _tenantRepository.DeleteAsync(volosoft);

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(volosoft.Id, null))).ShouldBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, volosoft.Name))).ShouldBeNull();

        var abp = await _tenantRepository.FindByNameAsync("abp");
        abp.ShouldNotBeNull();

        // Find will cache tenant.
        await _tenantStore.FindAsync(abp.Id);
        await _tenantStore.FindAsync(abp.Name);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(abp.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, abp.Name))).ShouldNotBeNull();

        await _tenantManager.ChangeNameAsync(abp, "abp2");
        await _tenantRepository.UpdateAsync(abp);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(abp.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, "abp"))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, "abp2"))).ShouldBeNull();
    }
}
