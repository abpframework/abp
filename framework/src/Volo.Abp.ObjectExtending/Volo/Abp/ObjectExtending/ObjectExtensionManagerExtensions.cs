using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class ObjectExtensionManagerExtensions
    {
        public static ObjectExtensionManager AddOrUpdateProperty<TObject, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null) 
            where TObject : IHasExtraProperties
        {
            return objectExtensionManager.AddOrUpdateProperty(
                typeof(TObject),
                typeof(TProperty),
                propertyName,
                configureAction
            );
        }

        public static ObjectExtensionManager AddOrUpdateProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type objectType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdate(
                objectType,
                options =>
                {
                    options.AddOrUpdateProperty(
                        propertyType,
                        propertyName,
                        configureAction
                    );
                });
        }
    }
}