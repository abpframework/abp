using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

public interface IDynamicFeatureDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(
        List<FeatureGroupDefinitionRecord> featureGroupRecords,
        List<FeatureDefinitionRecord> featureRecords);

    FeatureDefinition GetFeatureOrNull(string name);

    IReadOnlyList<FeatureDefinition> GetFeatures();

    IReadOnlyList<FeatureGroupDefinition> GetGroups();
}
