using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureDefinitionExtensions
    {
        public static void RequireGlobalFeatures(this PermissionDefinition permissionDefinition, params string[] globalFeatures)
        {
            Check.NotNullOrEmpty(globalFeatures, nameof(globalFeatures));

            permissionDefinition.AddStateProvider(new RequireGlobalFeaturesPermissionStateProvider(globalFeatures));
        }
    }
}
