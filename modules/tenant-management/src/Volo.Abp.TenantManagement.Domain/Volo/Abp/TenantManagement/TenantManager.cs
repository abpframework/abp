using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement;

public class TenantManager : DomainService, ITenantManager
{
    protected ITenantRepository TenantRepository { get; }
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; }

    protected ITenantNormalizer TenantNormalizer { get; }

    public TenantManager(ITenantRepository tenantRepository, IDistributedCache<TenantConfigurationCacheItem> cache, ITenantNormalizer tenantNormalizer)
    {
        TenantRepository = tenantRepository;
        Cache = cache;
        TenantNormalizer = tenantNormalizer;
    }

    public virtual async Task<Tenant> CreateAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var normalizedTenantName = TenantNormalizer.NormalizeName(name);
        await ValidateNameAsync(normalizedTenantName);
        return new Tenant(GuidGenerator.Create(), name, normalizedTenantName);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        var normalizedTenantName = TenantNormalizer.NormalizeName(name);

        await ValidateNameAsync(normalizedTenantName, tenant.Id);
        await Cache.RemoveAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenant.NormalizedName));
        tenant.SetName(name);
        tenant.SetNormalizedTenantName(normalizedTenantName);
    }

    protected virtual async Task ValidateNameAsync(string normalizeName, Guid? expectedId = null)
    {
        var tenant = await TenantRepository.FindByNameAsync(normalizeName);
        if (tenant != null && tenant.Id != expectedId)
        {
            throw new BusinessException("Volo.Abp.TenantManagement:DuplicateTenantName").WithData("Name", normalizeName);
        }
    }
}
