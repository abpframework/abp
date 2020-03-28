using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionManagerExtensions
    {
        public static ObjectExtensionManager MapEfCoreProperty<TObject, TProperty>(
            this ObjectExtensionManager objectExtensionManager,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
            where TObject : IHasExtraProperties
        {
            return objectExtensionManager.MapEfCoreProperty(
                typeof(TObject),
                typeof(TProperty),
                propertyName,
                propertyBuildAction
            );
        }

        public static ObjectExtensionManager MapEfCoreProperty(
            this ObjectExtensionManager objectExtensionManager,
            Type objectType,
            Type propertyType,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionManager.AddOrUpdateProperty(
                objectType,
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

    public static void ConfigureEfCoreEntity(
        this ObjectExtensionManager objectExtensionManager,
        EntityTypeBuilder b)
    {
        var objectExtension = objectExtensionManager.GetOrNull(b.Metadata.ClrType);
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
            if (b.Metadata.FindProperty(property.Name) != null)
            {
                continue;
            }

            var propertyBuilder = b.Property(property.Type, property.Name);

            efCoreMapping.PropertyBuildAction?.Invoke(propertyBuilder);
        }
    }
}
}
