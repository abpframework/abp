using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.PermissionManagement;

public class StaticPermissionSaver : IStaticPermissionSaver, ITransientDependency
{
    protected IStaticPermissionDefinitionStore StaticStore { get; }
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; }
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; }
    protected IPermissionDefinitionSerializer PermissionSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationNameAccessor ApplicationNameAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected PermissionManagementOptions PermissionManagementOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    
    public StaticPermissionSaver(
        IStaticPermissionDefinitionStore staticStore,
        IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
        IPermissionDefinitionRecordRepository permissionRepository,
        IPermissionDefinitionSerializer permissionSerializer,
        IDistributedCache cache, 
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationNameAccessor applicationNameAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<PermissionManagementOptions> permissionManagementOptions,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        StaticStore = staticStore;
        PermissionGroupRepository = permissionGroupRepository;
        PermissionRepository = permissionRepository;
        PermissionSerializer = permissionSerializer;
        Cache = cache;
        ApplicationNameAccessor = applicationNameAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        PermissionManagementOptions = permissionManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }
    
    [UnitOfWork]
    public virtual async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );
        
        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }
        
        /* NOTE: This can be further optimized by using 4 cache values for:
         * Groups, permissions, deleted groups and deleted permissions.
         * But the code would be more complex. This is enough for now.
         */

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var (permissionGroupRecords, permissionRecords) = await PermissionSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );

        var currentHash = CalculateHash(
            permissionGroupRecords,
            permissionRecords,
            PermissionManagementOptions.DeletedPermissionGroups,
            PermissionManagementOptions.DeletedPermissions
        );
        
        if (cachedHash == currentHash)
        {
            return;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(
                         GetCommonDistributedLockKey(),
                         TimeSpan.FromMinutes(5)))
        {
            if (commonLockHandle == null)
            {
                /* It will re-try */
                throw new AbpException("Could not acquire distributed lock for saving static permissions!");
            }

            var hasChangesInGroups = await UpdateChangedPermissionGroupsAsync(permissionGroupRecords);
            var hasChangesInPermissions = await UpdateChangedPermissionsAsync(permissionRecords);

            if (hasChangesInGroups ||hasChangesInPermissions)
            {
                await Cache.SetStringAsync(
                    GetCommonStampCacheKey(),
                    Guid.NewGuid().ToString(),
                    new DistributedCacheEntryOptions {
                        SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                    },
                    CancellationTokenProvider.Token
                );
            }
        }

        await Cache.SetStringAsync(
            cacheKey,
            currentHash,
            new DistributedCacheEntryOptions {
                SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
            },
            CancellationTokenProvider.Token
        );
    }

    private async Task<bool> UpdateChangedPermissionGroupsAsync(
        IEnumerable<PermissionGroupDefinitionRecord> permissionGroupRecords)
    {
        var newRecords = new List<PermissionGroupDefinitionRecord>();
        var changedRecords = new List<PermissionGroupDefinitionRecord>();

        var permissionGroupRecordsInDatabase = (await PermissionGroupRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroupRecordInDatabase = permissionGroupRecordsInDatabase.GetOrDefault(permissionGroupRecord.Name);
            if (permissionGroupRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(permissionGroupRecord);
                continue;
            }

            if (permissionGroupRecord.HasSameData(permissionGroupRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionGroupRecordInDatabase.Patch(permissionGroupRecord);
            changedRecords.Add(permissionGroupRecordInDatabase);
        }
        
        /* Deleted */
        var deletedRecords = PermissionManagementOptions.DeletedPermissionGroups.Any()
            ? permissionGroupRecordsInDatabase.Values
                .Where(x => PermissionManagementOptions.DeletedPermissionGroups.Contains(x.Name))
                .ToArray()
            : Array.Empty<PermissionGroupDefinitionRecord>();

        if (newRecords.Any())
        {
            await PermissionGroupRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await PermissionGroupRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await PermissionGroupRepository.DeleteManyAsync(deletedRecords);
        }
        
        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }
    
    private async Task<bool> UpdateChangedPermissionsAsync(
        IEnumerable<PermissionDefinitionRecord> permissionRecords)
    {
        var newRecords = new List<PermissionDefinitionRecord>();
        var changedRecords = new List<PermissionDefinitionRecord>();

        var permissionRecordsInDatabase = (await PermissionRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var permissionRecord in permissionRecords)
        {
            var permissionRecordInDatabase = permissionRecordsInDatabase.GetOrDefault(permissionRecord.Name);
            if (permissionRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(permissionRecord);
                continue;
            }

            if (permissionRecord.HasSameData(permissionRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            permissionRecordInDatabase.Patch(permissionRecord);
            changedRecords.Add(permissionRecordInDatabase);
        }
        
        /* Deleted */
        var deletedRecords = new List<PermissionDefinitionRecord>();
        
        if (PermissionManagementOptions.DeletedPermissions.Any())
        {
            deletedRecords.AddRange(
                permissionRecordsInDatabase.Values
                    .Where(x => PermissionManagementOptions.DeletedPermissions.Contains(x.Name))
            );
        }

        if (PermissionManagementOptions.DeletedPermissionGroups.Any())
        {
            deletedRecords.AddIfNotContains(
                permissionRecordsInDatabase.Values
                    .Where(x => PermissionManagementOptions.DeletedPermissionGroups.Contains(x.GroupName))
            );
        }

        if (newRecords.Any())
        {
            await PermissionRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await PermissionRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await PermissionRepository.DeleteManyAsync(deletedRecords);
        }
        
        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationNameAccessor.ApplicationName}_AbpPermissionUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpPermissionUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationNameAccessor.ApplicationName}_AbpPermissionsHash";
    }
    
    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryPermissionCacheStamp";
    }

    private static string CalculateHash(
        PermissionGroupDefinitionRecord[] permissionGroupRecords,
        PermissionDefinitionRecord[] permissionRecords,
        IEnumerable<string> deletedPermissionGroups,
        IEnumerable<string> deletedPermissions)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append("PermissionGroupRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(permissionGroupRecords));
        
        stringBuilder.Append("PermissionRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(permissionRecords));
        
        stringBuilder.Append("DeletedPermissionGroups:");
        stringBuilder.AppendLine(deletedPermissionGroups.JoinAsString(","));
        
        stringBuilder.Append("DeletedPermission:");
        stringBuilder.Append(deletedPermissions.JoinAsString(","));
        
        return stringBuilder
            .ToString()
            .ToMd5();
    }
}