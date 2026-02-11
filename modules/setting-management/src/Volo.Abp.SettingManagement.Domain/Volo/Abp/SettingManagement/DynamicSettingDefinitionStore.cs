using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement;

[Dependency(ReplaceServices = true)]
public class DynamicSettingDefinitionStore : IDynamicSettingDefinitionStore, ITransientDependency
{
    protected ISettingDefinitionRecordRepository SettingRepository { get; }
    protected ISettingDefinitionSerializer SettingDefinitionSerializer { get; }
    protected IDynamicSettingDefinitionStoreInMemoryCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    public SettingManagementOptions SettingManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public DynamicSettingDefinitionStore(
        ISettingDefinitionRecordRepository textSettingRepository,
        ISettingDefinitionSerializer textSettingDefinitionSerializer,
        IDynamicSettingDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<SettingManagementOptions> settingManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        SettingRepository = textSettingRepository;
        SettingDefinitionSerializer = textSettingDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        SettingManagementOptions = settingManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public virtual async Task<SettingDefinition> GetAsync(string name)
    {
        var setting = await GetOrNullAsync(name);
        if (setting == null)
        {
            throw new AbpException("Undefined setting: " + name);
        }

        return setting;
    }

    public virtual async Task<SettingDefinition> GetOrNullAsync(string name)
    {
        if (!SettingManagementOptions.IsDynamicSettingStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetSettingOrNull(name);
        }
    }

    public virtual async Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        if (!SettingManagementOptions.IsDynamicSettingStoreEnabled)
        {
            return Array.Empty<SettingDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetSettings().ToImmutableList();
        }
    }

    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
            /* We get the latest setting with a small delay for optimization */
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == StoreCache.CacheStamp)
        {
            StoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        StoreCache.CacheStamp = stampInDistributedCache;
        StoreCache.LastCheckTime = DateTime.Now;
    }

    protected virtual async Task UpdateInMemoryStoreCache()
    {
        var settingRecords = await SettingRepository.GetListAsync();
        await StoreCache.FillAsync(settingRecords);
    }

    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2)))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for setting definition common stamp check!"
                );
            }

            stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache;
            }

            stampInDistributedCache = Guid.NewGuid().ToString();

            await DistributedCache.SetStringAsync(
                cacheKey,
                stampInDistributedCache,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemorySettingCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpSettingUpdateLock";
    }
}
