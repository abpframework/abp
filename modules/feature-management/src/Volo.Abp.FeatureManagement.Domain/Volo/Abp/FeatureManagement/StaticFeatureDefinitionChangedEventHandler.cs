using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Features;
using Volo.Abp.StaticDefinitions;
using Volo.Abp.Threading;

namespace Volo.Abp.FeatureManagement;

public class StaticFeatureDefinitionChangedEventHandler : ILocalEventHandler<StaticFeatureDefinitionChangedEvent>, ITransientDependency
{
    protected IStaticDefinitionCache<FeatureGroupDefinition, Dictionary<string, FeatureGroupDefinition>> GroupCache { get; }
    protected IStaticDefinitionCache<FeatureDefinition, Dictionary<string, FeatureDefinition>> DefinitionCache { get; }
    protected FeatureDynamicInitializer FeatureDynamicInitializer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public StaticFeatureDefinitionChangedEventHandler(
        IStaticDefinitionCache<FeatureGroupDefinition, Dictionary<string, FeatureGroupDefinition>> groupCache,
        IStaticDefinitionCache<FeatureDefinition, Dictionary<string, FeatureDefinition>> definitionCache,
        FeatureDynamicInitializer featureDynamicInitializer,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        GroupCache = groupCache;
        DefinitionCache = definitionCache;
        FeatureDynamicInitializer = featureDynamicInitializer;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task HandleEventAsync(StaticFeatureDefinitionChangedEvent eventData)
    {
        await GroupCache.ClearAsync();
        await DefinitionCache.ClearAsync();
        await FeatureDynamicInitializer.InitializeAsync(false, CancellationTokenProvider.Token);
    }
}
