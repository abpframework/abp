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
            this ObjectExtensionPropertyInfo propertyExtension,
            Type dbFieldType,
            Action<PropertyBuilder> propertyBuildAction)
        {
            var options = new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                dbFieldType,
                propertyExtension,
                propertyBuildAction
            );

            propertyExtension.Configuration[EfCorePropertyConfigurationName] = options;

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
