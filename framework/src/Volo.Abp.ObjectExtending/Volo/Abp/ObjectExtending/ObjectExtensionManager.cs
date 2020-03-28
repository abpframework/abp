using System;
using System.Collections.Generic;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionManager
    {
        public static ObjectExtensionManager Instance { get; set; } = new ObjectExtensionManager();

        protected Dictionary<Type, ObjectExtensionInfo> ObjectsExtensions { get; }

        protected ObjectExtensionManager()
        {
            ObjectsExtensions = new Dictionary<Type, ObjectExtensionInfo>();
        }

        public virtual ObjectExtensionInfo AddOrUpdate<TObject>(
            Action<ObjectExtensionInfo> configureAction = null)
        {
            return AddOrUpdate(typeof(TObject), configureAction);
        }

        public virtual ObjectExtensionInfo AddOrUpdate(
            Type type,
            Action<ObjectExtensionInfo> configureAction = null)
        {
            var extensionInfo = ObjectsExtensions.GetOrAdd(
                type,
                () => new ObjectExtensionInfo(type)
            );

            configureAction?.Invoke(extensionInfo);

            return extensionInfo;
        }

        public virtual ObjectExtensionInfo GetOrNull<TObject>()
        {
            return GetOrNull(typeof(TObject));
        }

        public virtual ObjectExtensionInfo GetOrNull(Type type)
        {
            return ObjectsExtensions.GetOrDefault(type);
        }
    }
}