using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Features;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.FeatureManagement;

public class StaticFeatureSaver : IStaticFeatureSaver, ITransientDependency
{
    protected IStaticFeatureDefinitionStore StaticStore { get; }
    protected IFeatureGroupDefinitionRecordRepository FeatureGroupRepository { get; }
    protected IFeatureDefinitionRecordRepository FeatureRepository { get; }
    protected IFeatureDefinitionSerializer FeatureSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpFeatureOptions FeatureOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public StaticFeatureSaver(
        IStaticFeatureDefinitionStore staticStore,
        IFeatureGroupDefinitionRecordRepository featureGroupRepository,
        IFeatureDefinitionRecordRepository featureRepository,
        IFeatureDefinitionSerializer featureSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpFeatureOptions> featureManagementOptions,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        StaticStore = staticStore;
        FeatureGroupRepository = featureGroupRepository;
        FeatureRepository = featureRepository;
        FeatureSerializer = featureSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        FeatureOptions = featureManagementOptions.Value;
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
         * Groups, features, deleted groups and deleted features.
         * But the code would be more complex. This is enough for now.
         */

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var (featureGroupRecords, featureRecords) = await FeatureSerializer.SerializeAsync(
            await StaticStore.GetGroupsAsync()
        );

        var currentHash = CalculateHash(
            featureGroupRecords,
            featureRecords,
            FeatureOptions.DeletedFeatureGroups,
            FeatureOptions.DeletedFeatures
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
                throw new AbpException("Could not acquire distributed lock for saving static features!");
            }

            var hasChangesInGroups = await UpdateChangedFeatureGroupsAsync(featureGroupRecords);
            var hasChangesInFeatures = await UpdateChangedFeaturesAsync(featureRecords);

            if (hasChangesInGroups ||hasChangesInFeatures)
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

    private async Task<bool> UpdateChangedFeatureGroupsAsync(
        IEnumerable<FeatureGroupDefinitionRecord> featureGroupRecords)
    {
        var newRecords = new List<FeatureGroupDefinitionRecord>();
        var changedRecords = new List<FeatureGroupDefinitionRecord>();

        var featureGroupRecordsInDatabase = (await FeatureGroupRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var featureGroupRecord in featureGroupRecords)
        {
            var featureGroupRecordInDatabase = featureGroupRecordsInDatabase.GetOrDefault(featureGroupRecord.Name);
            if (featureGroupRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(featureGroupRecord);
                continue;
            }

            if (featureGroupRecord.HasSameData(featureGroupRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            featureGroupRecordInDatabase.Patch(featureGroupRecord);
            changedRecords.Add(featureGroupRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = FeatureOptions.DeletedFeatureGroups.Any()
            ? featureGroupRecordsInDatabase.Values
                .Where(x => FeatureOptions.DeletedFeatureGroups.Contains(x.Name))
                .ToArray()
            : Array.Empty<FeatureGroupDefinitionRecord>();

        if (newRecords.Any())
        {
            await FeatureGroupRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await FeatureGroupRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await FeatureGroupRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private async Task<bool> UpdateChangedFeaturesAsync(
        IEnumerable<FeatureDefinitionRecord> featureRecords)
    {
        var newRecords = new List<FeatureDefinitionRecord>();
        var changedRecords = new List<FeatureDefinitionRecord>();

        var featureRecordsInDatabase = (await FeatureRepository.GetListAsync())
            .ToDictionary(x => x.Name);

        foreach (var featureRecord in featureRecords)
        {
            var featureRecordInDatabase = featureRecordsInDatabase.GetOrDefault(featureRecord.Name);
            if (featureRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(featureRecord);
                continue;
            }

            if (featureRecord.HasSameData(featureRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            featureRecordInDatabase.Patch(featureRecord);
            changedRecords.Add(featureRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<FeatureDefinitionRecord>();

        if (FeatureOptions.DeletedFeatures.Any())
        {
            deletedRecords.AddRange(
                featureRecordsInDatabase.Values
                    .Where(x => FeatureOptions.DeletedFeatures.Contains(x.Name))
            );
        }

        if (FeatureOptions.DeletedFeatureGroups.Any())
        {
            deletedRecords.AddIfNotContains(
                featureRecordsInDatabase.Values
                    .Where(x => FeatureOptions.DeletedFeatureGroups.Contains(x.GroupName))
            );
        }

        if (newRecords.Any())
        {
            await FeatureRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await FeatureRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await FeatureRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpFeatureUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpFeatureUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpFeaturesHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryFeatureCacheStamp";
    }

    private static string CalculateHash(
        FeatureGroupDefinitionRecord[] featureGroupRecords,
        FeatureDefinitionRecord[] featureRecords,
        IEnumerable<string> deletedFeatureGroups,
        IEnumerable<string> deletedFeatures)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("FeatureGroupRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(featureGroupRecords));

        stringBuilder.Append("FeatureRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(featureRecords));

        stringBuilder.Append("DeletedFeatureGroups:");
        stringBuilder.AppendLine(deletedFeatureGroups.JoinAsString(","));

        stringBuilder.Append("DeletedFeature:");
        stringBuilder.Append(deletedFeatures.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
