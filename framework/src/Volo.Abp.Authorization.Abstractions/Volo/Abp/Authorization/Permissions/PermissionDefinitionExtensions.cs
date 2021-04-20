using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionDefinitionExtensions
    {
        public static PermissionDefinition AddStateProviders(
            [NotNull] this PermissionDefinition permissionDefinition,
            [NotNull] params ISimpleStateChecker<PermissionDefinition>[] permissionStateProviders)
        {
            Check.NotNull(permissionDefinition, nameof(permissionDefinition));
            Check.NotNull(permissionStateProviders, nameof(permissionStateProviders));

            permissionDefinition.SimpleStateCheckers.AddRange(permissionStateProviders);

            return permissionDefinition;
        }
    }
}
