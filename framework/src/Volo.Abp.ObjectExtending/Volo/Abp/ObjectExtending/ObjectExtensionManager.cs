using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public static class ObjectExtensionManager
    {
        //TODO: Concurrent, to allow extend on runtime!
        private static Dictionary<Type, ObjectExtensionInfo> Extensions { get; }

        static ObjectExtensionManager()
        {
            Extensions = new Dictionary<Type, ObjectExtensionInfo>();
        }

        public static ObjectExtensionPropertyInfo AddProperty<TDto>(
            string propertyName, 
            Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            var extensionInfo = Extensions.GetOrAdd(typeof(TDto), () => new ObjectExtensionInfo());
            var propertyInfo = extensionInfo.Properties.GetOrAdd(propertyName, () => new ObjectExtensionPropertyInfo(propertyName));
            configureAction?.Invoke(propertyInfo);
            return propertyInfo;
        }

        public static ImmutableList<ObjectExtensionPropertyInfo> GetProperties<TDto>()
            where TDto : IHasExtraProperties
        {
            var extensionInfo = Extensions.GetOrDefault(typeof(TDto));
            if (extensionInfo == null)
            {
                return new ObjectExtensionPropertyInfo[0].ToImmutableList(); //TODO: Return an empty one!
            }

            return extensionInfo.Properties.Values.ToImmutableList();
        }
    }
}