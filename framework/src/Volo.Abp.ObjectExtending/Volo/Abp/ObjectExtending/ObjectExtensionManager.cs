using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionManager
    {
        public static ObjectExtensionManager Instance { get; } = new ObjectExtensionManager();

        private Dictionary<Type, ObjectExtensionInfo> Extensions { get; }

        private ObjectExtensionManager()
        {
            Extensions = new Dictionary<Type, ObjectExtensionInfo>();
        }

        public ObjectExtensionPropertyInfo AddProperty<TDto>(
            string propertyName, 
            Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            var extensionInfo = Extensions.GetOrAdd(typeof(TDto), () => new ObjectExtensionInfo());
            var propertyInfo = extensionInfo.Properties.GetOrAdd(propertyName, () => new ObjectExtensionPropertyInfo(propertyName));
            configureAction?.Invoke(propertyInfo);
            return propertyInfo;
        }

        public ImmutableList<ObjectExtensionPropertyInfo> GetProperties<TDto>()
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