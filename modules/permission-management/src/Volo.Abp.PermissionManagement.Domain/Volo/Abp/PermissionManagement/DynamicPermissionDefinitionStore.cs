using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement;

[Dependency(ReplaceServices = true)]
public class DynamicPermissionDefinitionStore : IDynamicPermissionDefinitionStore, ITransientDependency
{
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; }
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; }
    protected IPermissionDefinitionSerializer PermissionDefinitionSerializer { get; }
    protected IDynamicPermissionDefinitionStoreInMemoryCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    
    public DynamicPermissionDefinitionStore(
        IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
        IPermissionDefinitionRecordRepository permissionRepository,
        IPermissionDefinitionSerializer permissionDefinitionSerializer,
        IDynamicPermissionDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions)
    {
        PermissionGroupRepository = permissionGroupRepository;
        PermissionRepository = permissionRepository;
        PermissionDefinitionSerializer = permissionDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        CacheOptions = cacheOptions.Value;
    }

    public virtual async Task<PermissionDefinition> GetOrNullAsync(string name)
    {
        await EnsureCacheIsUptoDateAsync();
        return StoreCache.GetPermissionOrNull(name);
    }

    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        await EnsureCacheIsUptoDateAsync();
        return StoreCache.GetPermissions();
    }

    public virtual async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        await EnsureCacheIsUptoDateAsync();
        return StoreCache.GetGroups();
    }

    private async Task EnsureCacheIsUptoDateAsync()
    {
        /* TODO: Optimization note: May not check for a few seconds
         * It is acceptable to get changes a few seconds after the last time the cache was updated.
         */
        
        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            var cacheKey = GetCacheKey();
        
            var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        
            if (StoreCache.CacheStamp == stampInDistributedCache)
            {
                return;
            }

            var permissionGroupRecords = await PermissionGroupRepository.GetListAsync();
            var permissionRecords = await PermissionRepository.GetListAsync();

            await StoreCache.FillAsync(permissionGroupRecords, permissionRecords);

            StoreCache.CacheStamp = stampInDistributedCache;
        }
    }

    private string GetCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryPermissionCacheStamp";
    }
}