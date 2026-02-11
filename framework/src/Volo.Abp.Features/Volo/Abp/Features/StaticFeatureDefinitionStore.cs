using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.StaticDefinitions;

namespace Volo.Abp.Features;

public class StaticFeatureDefinitionStore: IStaticFeatureDefinitionStore, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpFeatureOptions Options { get; }
    protected IStaticDefinitionCache<FeatureGroupDefinition, Dictionary<string, FeatureGroupDefinition>> GroupCache { get; }
    protected IStaticDefinitionCache<FeatureDefinition, Dictionary<string, FeatureDefinition>> DefinitionCache { get; }

    public StaticFeatureDefinitionStore(
        IServiceProvider serviceProvider,
        IOptions<AbpFeatureOptions> options,
        IStaticDefinitionCache<FeatureGroupDefinition, Dictionary<string, FeatureGroupDefinition>> groupCache,
        IStaticDefinitionCache<FeatureDefinition, Dictionary<string, FeatureDefinition>> definitionCache)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        GroupCache = groupCache;
        DefinitionCache = definitionCache;
    }

    public virtual async Task<FeatureDefinition> GetAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var feature = await GetOrNullAsync(name);

        if (feature == null)
        {
            throw new AbpException("Undefined feature: " + name);
        }

        return feature;
    }

    public virtual async Task<FeatureDefinition?> GetOrNullAsync(string name)
    {
        var defs = await GetFeatureDefinitionsAsync();
        return defs.GetOrDefault(name);
    }

    public virtual async Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync()
    {
        var defs = await GetFeatureDefinitionsAsync();
        return defs.Values.ToList();
    }

    public virtual async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        var groups = await GetFeatureGroupDefinitionsAsync();
        return groups.Values.ToList();
    }

    protected virtual async Task<Dictionary<string, FeatureGroupDefinition>> GetFeatureGroupDefinitionsAsync()
    {
        return await GroupCache.GetOrCreateAsync(CreateFeatureGroupDefinitionsAsync);
    }

    protected virtual async Task<Dictionary<string, FeatureDefinition>> GetFeatureDefinitionsAsync()
    {
        return await DefinitionCache.GetOrCreateAsync(CreateFeatureDefinitionsAsync);
    }

    protected virtual Task<Dictionary<string, FeatureGroupDefinition>> CreateFeatureGroupDefinitionsAsync()
    {
        var context = new FeatureDefinitionContext();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => (scope.ServiceProvider.GetRequiredService(p) as IFeatureDefinitionProvider)!)
                .ToList();

            foreach (var provider in providers)
            {
                provider.Define(context);
            }
        }

        return Task.FromResult(context.Groups);
    }

    protected virtual async Task<Dictionary<string, FeatureDefinition>> CreateFeatureDefinitionsAsync()
    {
        var features = new Dictionary<string, FeatureDefinition>();

        var groups = await GetFeatureGroupDefinitionsAsync();
        foreach (var groupDefinition in groups.Values)
        {
            foreach (var feature in groupDefinition.Features)
            {
                AddFeatureToDictionaryRecursively(features, feature);
            }
        }

        return features;
    }

    protected virtual void AddFeatureToDictionaryRecursively(
        Dictionary<string, FeatureDefinition> features,
        FeatureDefinition feature)
    {
        if (features.ContainsKey(feature.Name))
        {
            throw new AbpException("Duplicate feature name: " + feature.Name);
        }

        features[feature.Name] = feature;

        foreach (var child in feature.Children)
        {
            AddFeatureToDictionaryRecursively(features, child);
        }
    }
}
