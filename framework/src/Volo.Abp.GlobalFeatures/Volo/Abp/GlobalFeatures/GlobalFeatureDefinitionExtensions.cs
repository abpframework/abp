using System;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureDefinitionExtensions
    {
        public static PermissionDefinition RequireGlobalFeatures(
            this PermissionDefinition permissionDefinition,
            params string[] globalFeatures)
        {
            return permissionDefinition.RequireGlobalFeatures(true, globalFeatures);
        }

        public static PermissionDefinition RequireGlobalFeatures(
            [NotNull] this PermissionDefinition permissionDefinition,
            bool requiresAll,
            params string[] globalFeatures)
        {
            Check.NotNull(permissionDefinition, nameof(permissionDefinition));
            Check.NotNullOrEmpty(globalFeatures, nameof(globalFeatures));

            return permissionDefinition.AddStateProviders(
                new RequireGlobalFeaturesPermissionStateProvider(requiresAll, globalFeatures)
            );
        }

        public static PermissionDefinition RequireGlobalFeatures(
            this PermissionDefinition permissionDefinition,
            params Type[] globalFeatures)
        {
            return permissionDefinition.RequireGlobalFeatures(true, globalFeatures);
        }

        public static PermissionDefinition RequireGlobalFeatures(
            [NotNull] this PermissionDefinition permissionDefinition,
            bool requiresAll,
            params Type[] globalFeatures)
        {
            Check.NotNull(permissionDefinition, nameof(permissionDefinition));
            Check.NotNullOrEmpty(globalFeatures, nameof(globalFeatures));

            return permissionDefinition.AddStateProviders(
                new RequireGlobalFeaturesPermissionStateProvider(requiresAll, globalFeatures)
            );
        }

    }
}
