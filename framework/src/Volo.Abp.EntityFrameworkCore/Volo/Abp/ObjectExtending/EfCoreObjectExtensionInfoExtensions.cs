using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public static class EfCoreObjectExtensionInfoExtensions
    {
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
    }
}