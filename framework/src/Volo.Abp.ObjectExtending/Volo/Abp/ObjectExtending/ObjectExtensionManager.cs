using System;
using System.Collections.Generic;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionManager
    {
        public static ObjectExtensionManager Instance { get; } = new ObjectExtensionManager();

        private Dictionary<Type, ObjectExtensionInfo> ObjectsExtensions { get; }

        private ObjectExtensionManager()
        {
            ObjectsExtensions = new Dictionary<Type, ObjectExtensionInfo>();
        }

        public ObjectExtensionInfo For<TObject>(
            Action<ObjectExtensionInfo> configureAction = null)
        {
            var extensionInfo = ObjectsExtensions.GetOrAdd(
                typeof(TObject),
                () => new ObjectExtensionInfo(typeof(TObject))
            );

            configureAction?.Invoke(extensionInfo);

            return extensionInfo;
        }
    }
}