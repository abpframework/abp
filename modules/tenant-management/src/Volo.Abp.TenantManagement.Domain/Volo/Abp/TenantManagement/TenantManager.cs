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

        var normalizedName = TenantNormalizer.NormalizeName(name);
        await ValidateNameAsync(normalizedName);
        return new Tenant(GuidGenerator.Create(), name, normalizedName);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name);

        await ValidateNameAsync(normalizedName, tenant.Id);
        await Cache.RemoveAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenant.NormalizedName));
        tenant.SetName(name);
        tenant.SetNormalizedName(normalizedName);
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
