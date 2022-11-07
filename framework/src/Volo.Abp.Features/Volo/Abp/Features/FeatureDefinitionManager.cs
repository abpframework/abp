using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public class FeatureDefinitionManager : IFeatureDefinitionManager, ISingletonDependency
{
    protected IStaticFeatureDefinitionStore StaticStore;
    protected IDynamicFeatureDefinitionStore DynamicStore;

    public FeatureDefinitionManager(
        IStaticFeatureDefinitionStore staticStore,
        IDynamicFeatureDefinitionStore dynamicStore)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<FeatureDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        if (permission == null)
        {
            throw new AbpException("Undefined feature: " + name);
        }

        return permission;
    }

    public virtual async Task<FeatureDefinition> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await StaticStore.GetOrNullAsync(name) ??
               await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<FeatureDefinition>> GetAllAsync()
    {
        var staticFeatures = await StaticStore.GetFeaturesAsync();
        var staticFeatureNames = staticFeatures
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicFeatures = await DynamicStore.GetFeaturesAsync();

        /* We prefer static features over dynamics */
        return staticFeatures.Concat(
            dynamicFeatures.Where(d => !staticFeatureNames.Contains(d.Name))
        ).ToImmutableList();
    }

    public virtual async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await StaticStore.GetGroupsAsync();
        var staticGroupNames = staticGroups
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicGroups = await DynamicStore.GetGroupsAsync();

        /* We prefer static groups over dynamics */
        return staticGroups.Concat(
            dynamicGroups.Where(d => !staticGroupNames.Contains(d.Name))
        ).ToImmutableList();
    }
}
