using System;
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

            objectExtensionInfo.Configuration[EfCoreEntityConfigurationName] =
                new ObjectExtensionInfoEfCoreMappingOptions(
                    objectExtensionInfo,
                    entityTypeBuildAction);

            return objectExtensionInfo;
        }

        public static ObjectExtensionInfo MapEfCoreDbContext(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo,
            [NotNull] Action<ModelBuilder> modelBuildAction)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            objectExtensionInfo.Configuration[EfCoreDbContextConfigurationName] =
                new ObjectExtensionInfoEfCoreMappingOptions(
                    objectExtensionInfo,
                    modelBuildAction);

            return objectExtensionInfo;
        }

        [CanBeNull]
        public static ObjectExtensionInfoEfCoreMappingOptions GetEfCoreEntityMappingOrNull(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            if (!objectExtensionInfo.Configuration.TryGetValue(EfCoreEntityConfigurationName, out var options))
            {
                return null;
            }

            return options as ObjectExtensionInfoEfCoreMappingOptions;
        }

        [CanBeNull]
        public static ObjectExtensionInfoEfCoreMappingOptions GetEfCoreDbContextMappingOrNull(
            [NotNull] this ObjectExtensionInfo objectExtensionInfo)
        {
            Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));

            if (!objectExtensionInfo.Configuration.TryGetValue(EfCoreDbContextConfigurationName, out var options))
            {
                return null;
            }

            return options as ObjectExtensionInfoEfCoreMappingOptions;
        }
    }
}
