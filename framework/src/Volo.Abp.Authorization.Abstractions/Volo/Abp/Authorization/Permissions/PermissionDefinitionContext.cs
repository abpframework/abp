using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    public IServiceProvider ServiceProvider { get; }

    public Dictionary<string, PermissionGroupDefinition> Groups { get; }

    public List<PermissionDefinition> ResourcePermissions { get; }

    internal IPermissionDefinitionProvider? CurrentProvider { get; set; }

    public static class KnownPropertyNames
    {
        public const string CurrentProviderName = "_CurrentProviderName";
    }

    public PermissionDefinitionContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Groups = new Dictionary<string, PermissionGroupDefinition>();
        ResourcePermissions = new List<PermissionDefinition>();
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
        return group ?? throw new AbpException($"Could not find a permission definition group with the given name: {name}");
    }

    public virtual PermissionGroupDefinition? GetGroupOrNull([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));
        return Groups.GetOrDefault(name);
    }

    public virtual void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!Groups.Remove(name))
        {
            throw new AbpException($"Not found permission group with name: {name}");
        }
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

    public virtual PermissionDefinition AddResourcePermission(
        string name,
        string resourceName,
        string managementPermissionName,
        ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        bool isEnabled = true)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(resourceName, nameof(resourceName));
        Check.NotNull(managementPermissionName, nameof(managementPermissionName));

        if (ResourcePermissions.Any(x => x.ResourceName == resourceName && x.Name == name))
        {
            throw new AbpException($"There is already an existing resource permission with name: {name} for resource: {resourceName}");
        }

        var permission = new PermissionDefinition(
            name,
            resourceName,
            managementPermissionName,
            displayName,
            multiTenancySide,
            isEnabled)
        {
            [KnownPropertyNames.CurrentProviderName] = CurrentProvider?.GetType().FullName
        };

        ResourcePermissions.Add(permission);

        return permission;
    }

    public virtual PermissionDefinition? GetResourcePermissionOrNull([NotNull] string resourceName, [NotNull] string name)
    {
        Check.NotNull(resourceName, nameof(resourceName));
        Check.NotNull(name, nameof(name));

        return ResourcePermissions.FirstOrDefault(p => p.ResourceName == resourceName && p.Name == name);
    }

    public virtual void RemoveResourcePermission([NotNull] string resourceName, [NotNull] string name)
    {
        Check.NotNull(resourceName, nameof(resourceName));
        Check.NotNull(name, nameof(name));

        var resourcePermission = GetResourcePermissionOrNull(resourceName, name);
        if (resourcePermission == null)
        {
            throw new AbpException($"Not found resource permission with name: {name} for resource: {resourceName}");
        }
        ResourcePermissions.Remove(resourcePermission);
    }
}
