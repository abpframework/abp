using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Features
{
    public static class FeatureDefinitionExtensions
    {
        public static PermissionDefinition RequireFeatures(
            [NotNull] this PermissionDefinition permissionDefinition,
            params string[] features)
        {
            return permissionDefinition.RequireFeatures(true, features);
        }
        
        public static PermissionDefinition RequireFeatures(
            [NotNull] this PermissionDefinition permissionDefinition,
            bool requiresAll,
            params string[] features)
        {
            Check.NotNull(permissionDefinition, nameof(permissionDefinition));
            Check.NotNullOrEmpty(features, nameof(features));

            return permissionDefinition.AddStateProviders(
                new RequireFeaturesPermissionStateProvider(requiresAll, features)
            );
        }
    }
}
