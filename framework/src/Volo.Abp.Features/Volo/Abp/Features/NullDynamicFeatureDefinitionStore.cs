using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public class NullDynamicFeatureDefinitionStore : IDynamicFeatureDefinitionStore, ISingletonDependency
{
    private static readonly Task<FeatureDefinition?> CachedFeatureResult = Task.FromResult((FeatureDefinition?)null);

    private static readonly Task<IReadOnlyList<FeatureDefinition>> CachedFeaturesResult =
        Task.FromResult((IReadOnlyList<FeatureDefinition>)Array.Empty<FeatureDefinition>().ToImmutableList());

    private static readonly Task<IReadOnlyList<FeatureGroupDefinition>> CachedGroupsResult =
        Task.FromResult((IReadOnlyList<FeatureGroupDefinition>)Array.Empty<FeatureGroupDefinition>().ToImmutableList());

    public Task<FeatureDefinition?> GetOrNullAsync(string name)
    {
        return CachedFeatureResult;
    }

    public Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync()
    {
        return CachedFeaturesResult;
    }

    public Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        return CachedGroupsResult;
    }
}
