using System;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermissionDefinitionContext
{
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets a pre-defined permission group.
    /// Throws <see cref="AbpException"/> if can not find the given group.
    /// </summary>
    /// <param name="name">Name of the group</param>
    /// <returns></returns>
    PermissionGroupDefinition GetGroup(string name);

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if it cannot find the given group.
    /// </summary>
    /// <param name="name">Name of the group</param>
    /// <returns></returns>
    PermissionGroupDefinition? GetGroupOrNull(string name);

    /// <summary>
    /// Tries to add a new permission group.
    /// Throws <see cref="AbpException"/> if there is a group with the name.
    /// <param name="name">Name of the group</param>
    /// <param name="displayName">Localized display name of the group</param>
    /// </summary>
    PermissionGroupDefinition AddGroup(
        string name,
        ILocalizableString? displayName = null);

    /// <summary>
    /// Tries to remove a permission group.
    /// Throws <see cref="AbpException"/> if there is not any group with the name.
    /// <param name="name">Name of the group</param>
    /// </summary>
    void RemoveGroup(string name);

    /// <summary>
    /// Tries to get a pre-defined permission from all defined groups.
    /// Returns null if it cannot find the given permission.
    /// <param name="name">Name of the permission</param>
    /// </summary>
    PermissionDefinition? GetPermissionOrNull(string name);

    PermissionDefinition AddResourcePermission(
        string name,
        string resourceName,
        string managementPermissionName,
        ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        bool isEnabled = true);

    PermissionDefinition? GetResourcePermissionOrNull([NotNull] string resourceName, [NotNull] string name);

    void RemoveResourcePermission([NotNull] string resourceName, [NotNull] string name);
}
