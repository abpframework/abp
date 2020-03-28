using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionInfoExtensions
    {
        public static ObjectExtensionPropertyInfo MapEfCoreProperty<TDbField>(
            this ObjectExtensionInfo objectExtensionInfo,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionInfo.MapEfCoreProperty(
                typeof(TDbField),
                propertyName,
                propertyBuildAction
            );
        }

        public static ObjectExtensionPropertyInfo MapEfCoreProperty(
            this ObjectExtensionInfo objectExtensionInfo,
            Type dbFieldType,
            string propertyName,
            Action<PropertyBuilder> propertyBuildAction)
        {
            return objectExtensionInfo.AddOrUpdateProperty(
                propertyName,
                options =>
                {
                    options.MapEfCore(
                        dbFieldType, 
                        propertyBuildAction
                    );
                });
        }
    }
}