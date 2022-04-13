using System;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermissionDefinitionContext
{
    //TODO: Add Get methods to find and modify a permission or group.

    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets a pre-defined permission group.
    /// Throws <see cref="AbpException"/> if can not find the given group.
    /// </summary>
    /// <param name="name">Name of the group</param>
    /// <returns></returns>
    PermissionGroupDefinition GetGroup([NotNull] string name);

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// </summary>
    /// <param name="name">Name of the group</param>
    /// <returns></returns>
    [CanBeNull]
    PermissionGroupDefinition GetGroupOrNull(string name);

    /// <summary>
    /// Tries to add a new permission group.
    /// Throws <see cref="AbpException"/> if there is a group with the name.
    /// <param name="name">Name of the group</param>
    /// <param name="displayName">Localized display name of the group</param>
    /// <param name="multiTenancySide">Select a multi-tenancy side</param>
    /// </summary>
    PermissionGroupDefinition AddGroup(
        [NotNull] string name,
        ILocalizableString displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both);

    /// <summary>
    /// Tries to remove a permission group.
    /// Throws <see cref="AbpException"/> if there is not any group with the name.
    /// <param name="name">Name of the group</param>
    /// </summary>
    void RemoveGroup(string name);

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// <param name="name">Name of the group</param>
    /// </summary>
    [CanBeNull]
    PermissionDefinition GetPermissionOrNull([NotNull] string name);
}
