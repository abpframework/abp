using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionDefinitionExtensions
    {
        public const string PropertyName = "_AbpPermissionStateProviders";

        public static PermissionDefinition AddStateProvider(
            [NotNull] this PermissionDefinition permissionDefinition,
            [NotNull] params IPermissionStateProvider[] permissionStateProviders)
        {
            var stateProviders = permissionDefinition.GetStateProvidersInternal();

            foreach (var provider in permissionStateProviders)
            {
                stateProviders.AddIfNotContains(provider);
            }

            return permissionDefinition;
        }

        public static IReadOnlyList<IPermissionStateProvider> GetStateProviders([NotNull] this PermissionDefinition permissionDefinition)
        {
            return permissionDefinition.GetStateProvidersInternal();
        }

        private static List<IPermissionStateProvider> GetStateProvidersInternal([NotNull] this PermissionDefinition permissionDefinition)
        {
            return (List<IPermissionStateProvider>) permissionDefinition.Properties.GetOrAdd(PropertyName, () => new List<IPermissionStateProvider>());
        }
    }
}
