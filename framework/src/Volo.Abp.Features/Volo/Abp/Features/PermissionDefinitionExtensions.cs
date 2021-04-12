using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Features
{
    public static class FeatureDefinitionExtensions
    {
        public static void RequireFeatures(this PermissionDefinition permissionDefinition, params string[] features)
        {
            Check.NotNullOrEmpty(features, nameof(features));

            permissionDefinition.AddStateProvider(new RequireFeaturesPermissionStateProvider(features));
        }
    }
}
