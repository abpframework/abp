using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class ObjectExtensionManagerExtensions
    {
        [NotNull]
        public static ObjectExtensionManager AddOrUpdateProperty<TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type[] objectTypes,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            return objectExtensionManager.AddOrUpdateProperty(
                objectTypes,
                typeof(TProperty),
                propertyName, configureAction
            );
        }

        [NotNull]
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

        [NotNull]
        public static ObjectExtensionManager AddOrUpdateProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type[] objectTypes,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            Check.NotNull(objectTypes, nameof(objectTypes));

            foreach (var objectType in objectTypes)
            {
                objectExtensionManager.AddOrUpdateProperty(
                    objectType,
                    propertyType,
                    propertyName,
                    configureAction
                );
            }

            return objectExtensionManager;
        }

        [NotNull]
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

        public static ObjectExtensionPropertyInfo GetPropertyOrNull<TObject>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName)
        {
            return objectExtensionManager.GetPropertyOrNull(
                typeof(TObject),
                propertyName
            );
        }

        public static ObjectExtensionPropertyInfo GetPropertyOrNull(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type objectType,
            [NotNull] string propertyName)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
            Check.NotNull(objectType, nameof(objectType));
            Check.NotNull(propertyName, nameof(propertyName));

            return objectExtensionManager
                .GetOrNull(objectType)?
                .GetPropertyOrNull(propertyName);
        }

        private static readonly ImmutableList<ObjectExtensionPropertyInfo> EmptyPropertyList 
            = new List<ObjectExtensionPropertyInfo>().ToImmutableList();

        public static ImmutableList<ObjectExtensionPropertyInfo> GetProperties<TObject>(
            [NotNull] this ObjectExtensionManager objectExtensionManager)
        {
            return objectExtensionManager.GetProperties(typeof(TObject));
        }

        public static ImmutableList<ObjectExtensionPropertyInfo> GetProperties(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type objectType)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
            Check.NotNull(objectType, nameof(objectType));

            var extensionInfo = objectExtensionManager.GetOrNull(objectType);
            if (extensionInfo == null)
            {
                return EmptyPropertyList;
            }

            return extensionInfo.GetProperties();
        }
    }
}