using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionDefinitionExtensions
    {
        public static PermissionDefinition AddStateProviders(
            [NotNull] this PermissionDefinition permissionDefinition,
            [NotNull] params IPermissionStateProvider[] permissionStateProviders)
        {
            Check.NotNull(permissionDefinition, nameof(permissionDefinition));
            Check.NotNull(permissionStateProviders, nameof(permissionStateProviders));
            
            permissionDefinition.StateProviders.AddRange(permissionStateProviders);
            
            return permissionDefinition;
        }
    }
}
