using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.Abp.Features;

public class FeatureDefinitionContext : IFeatureDefinitionContext
{
    internal Dictionary<string, FeatureGroupDefinition> Groups { get; }

    public FeatureDefinitionContext()
    {
        Groups = new Dictionary<string, FeatureGroupDefinition>();
    }

    public FeatureGroupDefinition AddGroup(string name, ILocalizableString displayName = null)
    {
        Check.NotNull(name, nameof(name));

        if (Groups.ContainsKey(name))
        {
            throw new AbpException($"There is already an existing permission group with name: {name}");
        }

        return Groups[name] = new FeatureGroupDefinition(name, displayName);
    }

    public FeatureGroupDefinition GetGroupOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            return null;
        }

        return Groups[name];
    }

    public void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            throw new AbpException($"Undefined feature group: '{name}'.");
        }

        Groups.Remove(name);
    }
}
