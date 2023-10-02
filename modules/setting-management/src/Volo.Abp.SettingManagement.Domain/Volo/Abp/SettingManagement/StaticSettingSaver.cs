using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement;

public class StaticSettingSaver : IStaticSettingSaver, ITransientDependency
{
    protected IStaticSettingDefinitionStore StaticStore { get; }
    protected ISettingDefinitionRecordRepository SettingRepository { get; }
    protected ISettingDefinitionSerializer SettingSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpSettingOptions SettingOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public StaticSettingSaver(
        IStaticSettingDefinitionStore staticStore,
        ISettingDefinitionRecordRepository settingRepository,
        ISettingDefinitionSerializer settingSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpSettingOptions> settingOptions,
        ICancellationTokenProvider cancellationTokenProvider,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
    {
        StaticStore = staticStore;
        SettingRepository = settingRepository;
        SettingSerializer = settingSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        SettingOptions = settingOptions.Value;
        CacheOptions = cacheOptions.Value;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var settingRecords = await SettingSerializer.SerializeAsync(await StaticStore.GetAllAsync());
        var currentHash = CalculateHash(settingRecords, SettingOptions.DeletedSettings);

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
                throw new AbpException("Could not acquire distributed lock for saving static Settings!");
            }

            using (var unitOfWork = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                try
                {
                    var hasChangesInSettings = await UpdateChangedSettingsAsync(settingRecords);

                    if (hasChangesInSettings)
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
                catch
                {
                    try
                    {
                        await unitOfWork.RollbackAsync();
                    }
                    catch
                    {
                        /* ignored */
                    }

                    throw;
                }

                await unitOfWork.CompleteAsync();
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

    private async Task<bool> UpdateChangedSettingsAsync(List<SettingDefinitionRecord> SettingRecords)
    {
        var newRecords = new List<SettingDefinitionRecord>();
        var changedRecords = new List<SettingDefinitionRecord>();

        var settingRecordsInDatabase = (await SettingRepository.GetListAsync()).ToDictionary(x => x.Name);

        foreach (var record in SettingRecords)
        {
            var settingRecordInDatabase = settingRecordsInDatabase.GetOrDefault(record.Name);
            if (settingRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(record);
                continue;
            }

            if (record.HasSameData(settingRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            settingRecordInDatabase.Patch(record);
            changedRecords.Add(settingRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<SettingDefinitionRecord>();

        if (SettingOptions.DeletedSettings.Any())
        {
            deletedRecords.AddRange(settingRecordsInDatabase.Values.Where(x => SettingOptions.DeletedSettings.Contains(x.Name)));
        }

        if (newRecords.Any())
        {
            await SettingRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await SettingRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await SettingRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpSettingUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpSettingUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpSettingsHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemorySettingCacheStamp";
    }

    private string CalculateHash(List<SettingDefinitionRecord> settingRecords, IEnumerable<string> deletedSettings)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<SettingDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                }
            }
        };

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("SettingRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(settingRecords, jsonSerializerOptions));

        stringBuilder.Append("DeletedSetting:");
        stringBuilder.Append(deletedSettings.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
