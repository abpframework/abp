using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace Volo.Abp.FeatureManagement;

public class DynamicFeatureDefinitionStoreInMemoryCache:
    IDynamicFeatureDefinitionStoreInMemoryCache,
    ISingletonDependency
{
    public string CacheStamp { get; set; }

    protected IDictionary<string, FeatureGroupDefinition> FeatureGroupDefinitions { get; }
    protected IDictionary<string, FeatureDefinition> FeatureDefinitions { get; }
    protected StringValueTypeSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicFeatureDefinitionStoreInMemoryCache(
        StringValueTypeSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;

        FeatureGroupDefinitions = new Dictionary<string, FeatureGroupDefinition>();
        FeatureDefinitions = new Dictionary<string, FeatureDefinition>();
    }

    public Task FillAsync(
        List<FeatureGroupDefinitionRecord> featureGroupRecords,
        List<FeatureDefinitionRecord> featureRecords)
    {
        FeatureGroupDefinitions.Clear();
        FeatureDefinitions.Clear();

        var context = new FeatureDefinitionContext();

        foreach (var featureGroupRecord in featureGroupRecords)
        {
            var featureGroup = context.AddGroup(
                featureGroupRecord.Name,
                LocalizableStringSerializer.Deserialize(featureGroupRecord.DisplayName)
            );

            FeatureGroupDefinitions[featureGroup.Name] = featureGroup;

            foreach (var property in featureGroupRecord.ExtraProperties)
            {
                featureGroup[property.Key] = property.Value;
            }

            var featureRecordsInThisGroup = featureRecords
                .Where(p => p.GroupName == featureGroup.Name);

            foreach (var featureRecord in featureRecordsInThisGroup.Where(x => x.ParentName == null))
            {
                AddFeatureRecursively(featureGroup, featureRecord, featureRecords);
            }
        }

        return Task.CompletedTask;
    }

    public FeatureDefinition GetFeatureOrNull(string name)
    {
        return FeatureDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<FeatureDefinition> GetFeatures()
    {
        return FeatureDefinitions.Values.ToList();
    }

    public IReadOnlyList<FeatureGroupDefinition> GetGroups()
    {
        return FeatureGroupDefinitions.Values.ToList();
    }

    private void AddFeatureRecursively(ICanCreateChildFeature featureContainer,
        FeatureDefinitionRecord featureRecord,
        List<FeatureDefinitionRecord> allFeatureRecords)
    {
        var feature = featureContainer.CreateChildFeature(
            featureRecord.Name,
            featureRecord.DefaultValue,
            LocalizableStringSerializer.Deserialize(featureRecord.DisplayName),
            LocalizableStringSerializer.Deserialize(featureRecord.Description),
            StateCheckerSerializer.Deserialize(featureRecord.ValueType),
            featureRecord.IsVisibleToClients,
            featureRecord.IsAvailableToHost
        );

        FeatureDefinitions[feature.Name] = feature;

        if (!featureRecord.AllowedProviders.IsNullOrWhiteSpace())
        {
            feature.AllowedProviders.AddRange(featureRecord.AllowedProviders.Split(','));
        }

        foreach (var property in featureRecord.ExtraProperties)
        {
            feature[property.Key] = property.Value;
        }

        foreach (var subFeature in allFeatureRecords.Where(p => p.ParentName == featureRecord.Name))
        {
            AddFeatureRecursively(feature, subFeature, allFeatureRecords);
        }
    }
}
