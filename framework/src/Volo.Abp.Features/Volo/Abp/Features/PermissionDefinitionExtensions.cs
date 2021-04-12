using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Features
{
    public static class FeatureDefinitionExtensions
    {
        public static PermissionDefinition RequireFeatures(this PermissionDefinition permissionDefinition, params string[] features)
        {
            Check.NotNullOrEmpty(features, nameof(features));

            return permissionDefinition.AddStateProvider(new RequireFeaturesPermissionStateProvider(features));
        }
    }
}
