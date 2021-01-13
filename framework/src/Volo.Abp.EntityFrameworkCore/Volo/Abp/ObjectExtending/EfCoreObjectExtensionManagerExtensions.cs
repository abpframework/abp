using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionManagerExtensions
    {
        public static ObjectExtensionManager MapEfCoreProperty<TEntity, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName)
            where TEntity : IHasExtraProperties, IEntity
        {
            return objectExtensionManager.MapEfCoreProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName
            );
        }

        public static ObjectExtensionManager MapEfCoreProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdateProperty(
                entityType,
                propertyType,
                propertyName,
                options =>
                {
                    options.MapEfCore();
                }
            );
        }

        [Obsolete("Use MapEfCoreProperty with EntityTypeAndPropertyBuildAction parameters.")]
        public static ObjectExtensionManager MapEfCoreProperty<TEntity, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction)
            where TEntity : IHasExtraProperties, IEntity
        {
            return objectExtensionManager.MapEfCoreProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName,
                propertyBuildAction
            );
        }

        [Obsolete("Use MapEfCoreProperty with EntityTypeAndPropertyBuildAction parameters.")]
        public static ObjectExtensionManager MapEfCoreProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdateProperty(
                entityType,
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

        public static ObjectExtensionManager MapEfCoreProperty<TEntity, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
            where TEntity : IHasExtraProperties, IEntity
        {
            return objectExtensionManager.MapEfCoreProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName,
                entityTypeAndPropertyBuildAction
            );
        }

        public static ObjectExtensionManager MapEfCoreProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdateProperty(
                entityType,
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

        public static void ConfigureEfCoreEntity(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] EntityTypeBuilder typeBuilder)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
            Check.NotNull(typeBuilder, nameof(typeBuilder));

            var objectExtension = objectExtensionManager.GetOrNull(typeBuilder.Metadata.ClrType);
            if (objectExtension == null)
            {
                return;
            }

            foreach (var property in objectExtension.GetProperties())
            {
                var efCoreMapping = property.GetEfCoreMappingOrNull();
                if (efCoreMapping == null)
                {
                    continue;
                }

                /* Prevent multiple calls to the entityTypeBuilder.Property(...) method */
                if (typeBuilder.Metadata.FindProperty(property.Name) != null)
                {
                    continue;
                }

                var propertyBuilder = typeBuilder.Property(property.Type, property.Name);

                efCoreMapping.EntityTypeAndPropertyBuildAction?.Invoke(typeBuilder, propertyBuilder);
#pragma warning disable 618
                efCoreMapping.PropertyBuildAction?.Invoke(propertyBuilder);
#pragma warning restore 618
            }
        }
    }
}
