using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionPropertyInfoExtensions
    {
        public const string EfCorePropertyConfigurationName = "EfCoreMapping";

        public static ObjectExtensionPropertyInfo MapEfCore(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction = null)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            propertyExtension.Configuration[EfCorePropertyConfigurationName] =
                new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                    propertyExtension,
                    propertyBuildAction
                );

            return propertyExtension;
        }

        [CanBeNull]
        public static ObjectExtensionPropertyInfoEfCoreMappingOptions GetEfCoreMappingOrNull(
            this ObjectExtensionPropertyInfo propertyExtension)
        {
            return propertyExtension.Configuration.GetOrDefault(EfCorePropertyConfigurationName)
                as ObjectExtensionPropertyInfoEfCoreMappingOptions;
        }

        public static bool IsMappedToFieldForEfCore(this ObjectExtensionPropertyInfo propertyExtension)
        {
            return propertyExtension.Configuration.ContainsKey(EfCorePropertyConfigurationName);
        }
    }
}
