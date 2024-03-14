using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.TenantManagement;

public class TenantStore : ITenantStore, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }
    protected IObjectMapper<AbpTenantManagementDomainModule> ObjectMapper { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; }

    public TenantStore(
        ITenantRepository tenantRepository,
        IObjectMapper<AbpTenantManagementDomainModule> objectMapper,
        ICurrentTenant currentTenant,
        IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        TenantRepository = tenantRepository;
        ObjectMapper = objectMapper;
        CurrentTenant = currentTenant;
        Cache = cache;
    }

    public virtual async Task<TenantConfiguration> FindAsync(string normalizedName)
    {
        return (await GetCacheItemAsync(null, normalizedName)).Value;
    }

    public virtual async Task<TenantConfiguration> FindAsync(Guid id)
    {
        return (await GetCacheItemAsync(id, null)).Value;
    }

    public virtual async Task<IReadOnlyList<TenantConfiguration>> GetListAsync(bool includeDetails = false)
    {
        return ObjectMapper.Map<List<Tenant>, List<TenantConfiguration>>(
            await TenantRepository.GetListAsync(includeDetails));
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(string normalizedName)
    {
        return (GetCacheItem(null, normalizedName)).Value;
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(Guid id)
    {
        return (GetCacheItem(id, null)).Value;
    }

    protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemAsync(Guid? id, string normalizedName)
    {
        var cacheKey = CalculateCacheKey(id, normalizedName);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem?.Value != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        if (!normalizedName.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindByNameAsync(normalizedName);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and normalizedName can't be invalid.");
    }

    protected virtual async Task<TenantConfigurationCacheItem> SetCacheAsync(string cacheKey, [CanBeNull] Tenant tenant)
    {
        var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
        var cacheItem = new TenantConfigurationCacheItem(tenantConfiguration);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    [Obsolete("Use GetCacheItemAsync method.")]
    protected virtual TenantConfigurationCacheItem GetCacheItem(Guid? id, string normalizedName)
    {
        var cacheKey = CalculateCacheKey(id, normalizedName);

        var cacheItem = Cache.Get(cacheKey, considerUow: true);
        if (cacheItem?.Value != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindById(id.Value);
                return SetCache(cacheKey, tenant);
            }
        }

        if (!normalizedName.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindByName(normalizedName);
                return SetCache(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and normalizedName can't be invalid.");
    }

    [Obsolete("Use SetCacheAsync method.")]
    protected virtual TenantConfigurationCacheItem SetCache(string cacheKey, [CanBeNull] Tenant tenant)
    {
        var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
        var cacheItem = new TenantConfigurationCacheItem(tenantConfiguration);
        Cache.Set(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    protected virtual string CalculateCacheKey(Guid? id, string normalizedName)
    {
        return TenantConfigurationCacheItem.CalculateCacheKey(id, normalizedName);
    }
}
