using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionInfoExtensions
    {
        public const string EfCoreDbContextConfigurationName = "EfCoreDbContextMapping";
        public const string EfCoreEntityConfigurationName = "EfCoreEntityMapping";

        [Obsolete("Use MapEfCoreProperty with EntityTypeAndPropertyBuildAction parameters.")]
        public static ObjectExtensionInfo MapEfCoreProperty<TProperty>(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] string propertyName,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionInfo.MapEfCoreProperty(
                typeof(TProperty),
                propertyName,
                propertyBuildAction
            );
        }

        [Obsolete("Use MapEfCoreProperty with EntityTypeAndPropertyBuildAction parameters.")]
        public static ObjectExtensionInfo MapEfCoreProperty(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return objectExtensionInfo.AddOrUpdateProperty(
                propertyType,
                propertyName,
                options =>
                {
                    options.MapEfCore(
                        propertyBuildAction
                    );
                }
            );
        }

        public static ObjectExtensionInfo MapEfCoreProperty<TProperty>(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            return objectExtensionInfo.MapEfCoreProperty(
                typeof(TProperty),
                propertyName,
                entityTypeAndPropertyBuildAction
            );
        }

        public static ObjectExtensionInfo MapEfCoreProperty(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return objectExtensionInfo.AddOrUpdateProperty(
                propertyType,
                propertyName,
                options =>
                {
                    options.MapEfCore(
                        entityTypeAndPropertyBuildAction
                    );
                }
            );
        }

        public static ObjectExtensionInfo MapEfCoreEntity(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            var mappingOptionList = new List<ObjectExtensionInfoEfCoreMappingOptions>
            {
                new ObjectExtensionInfoEfCoreMappingOptions(
                    objectExtensionInfo,
                    entityTypeBuildAction)
            };

            objectExtensionInfo.Configuration.AddOrUpdate(EfCoreEntityConfigurationName, mappingOptionList,
                (k, v) =>
                {
                    v.As<List<ObjectExtensionInfoEfCoreMappingOptions>>().Add(mappingOptionList.First());
                    return v;
                });

            return objectExtensionInfo;
        }

        public static ObjectExtensionInfo MapEfCoreDbContext(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Action<ModelBuilder> modelBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            var mappingOptionList = new List<ObjectExtensionInfoEfCoreMappingOptions>
            {
                new ObjectExtensionInfoEfCoreMappingOptions(
                    objectExtensionInfo,
                    modelBuildAction)
            };

            objectExtensionInfo.Configuration.AddOrUpdate(EfCoreDbContextConfigurationName, mappingOptionList,
                (k, v) =>
                {
                    v.As<List<ObjectExtensionInfoEfCoreMappingOptions>>().Add(mappingOptionList.First());
                    return v;
                });

            return objectExtensionInfo;
        }

        public static List<ObjectExtensionInfoEfCoreMappingOptions> GetEfCoreEntityMappings(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return !objectExtensionInfo.Configuration.TryGetValue(EfCoreEntityConfigurationName, out var options) ?
                new List<ObjectExtensionInfoEfCoreMappingOptions>() : options.As<List<ObjectExtensionInfoEfCoreMappingOptions>>();
        }

        public static List<ObjectExtensionInfoEfCoreMappingOptions> GetEfCoreDbContextMappings(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            return !objectExtensionInfo.Configuration.TryGetValue(EfCoreDbContextConfigurationName, out var options) ?
                new List<ObjectExtensionInfoEfCoreMappingOptions>() : options.As<List<ObjectExtensionInfoEfCoreMappingOptions>>();
        }
    }
}
