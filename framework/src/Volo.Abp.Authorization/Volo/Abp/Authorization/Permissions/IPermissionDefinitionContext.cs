using System;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions
{
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
        [NotNull]
        PermissionGroupDefinition GetGroupOrNull(string name);

        [CanBeNull]
        PermissionGroupDefinition AddGroup(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both);

        void RemoveGroup(string name);

        [CanBeNull]
        PermissionDefinition GetPermissionOrNull([NotNull] string name);
    }
}