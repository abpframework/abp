using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionManagerExtensions
    {
        public static ObjectExtensionInfo MapEfCoreProperty<TObject, TDbField>(
            this ObjectExtensionManager objectExtensionManager,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionManager.MapEfCoreProperty(
                typeof(TObject),
                typeof(TDbField),
                propertyName,
                propertyBuildAction
            );
        }

        public static ObjectExtensionInfo MapEfCoreProperty(
            this ObjectExtensionManager objectExtensionManager,
            Type objectType,
            Type dbFieldType,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionManager.AddOrUpdate(
                objectType,
                objectOptions =>
                {
                    objectOptions.AddOrUpdateProperty(
                        propertyName,
                        propertyOptions =>
                        {
                            propertyOptions.MapEfCore(
                                dbFieldType,
                                propertyBuildAction
                            );
                        }
                    );
                });
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

                var propertyBuilder = b.Property(efCoreMapping.FieldType, property.Name);

                efCoreMapping.PropertyBuildAction?.Invoke(propertyBuilder);
            }
        }
    }
}
