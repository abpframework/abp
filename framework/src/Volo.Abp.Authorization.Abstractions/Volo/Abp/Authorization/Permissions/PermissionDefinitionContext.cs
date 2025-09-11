using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    public IServiceProvider ServiceProvider { get; }

    public Dictionary<string, PermissionGroupDefinition> Groups { get; }

    internal IPermissionDefinitionProvider? CurrentProvider { get; set; }

    public static class KnownPropertyNames
    {
        public const string CurrentProviderName = "_CurrentProviderName";
    }
    
    public PermissionDefinitionContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Groups = new Dictionary<string, PermissionGroupDefinition>();
    }

    public virtual PermissionGroupDefinition AddGroup(
        string name,
        ILocalizableString? displayName = null)
    {
        Check.NotNull(name, nameof(name));

        if (Groups.ContainsKey(name))
        {
            throw new AbpException($"There is already an existing permission group with name: {name}");
        }

        var group = new PermissionGroupDefinition(name, displayName);

        if (CurrentProvider != null)
        {
            group[KnownPropertyNames.CurrentProviderName] = CurrentProvider.GetType().FullName;
        }

        Groups[name] = group;
        
        return group;
    }

    [NotNull]
    public virtual PermissionGroupDefinition GetGroup([NotNull] string name)
    {
        var group = GetGroupOrNull(name);

        if (group == null)
        {
            throw new AbpException($"Could not find a permission definition group with the given name: {name}");
        }

        return group;
    }

    public virtual PermissionGroupDefinition? GetGroupOrNull([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            return null;
        }

        return Groups[name];
    }

    public virtual void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.ContainsKey(name))
        {
            throw new AbpException($"Not found permission group with name: {name}");
        }

        Groups.Remove(name);
    }

    public virtual PermissionDefinition? GetPermissionOrNull([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        foreach (var groupDefinition in Groups.Values)
        {
            var permissionDefinition = groupDefinition.GetPermissionOrNull(name);

            if (permissionDefinition != null)
            {
                return permissionDefinition;
            }
        }

        return null;
    }
}
