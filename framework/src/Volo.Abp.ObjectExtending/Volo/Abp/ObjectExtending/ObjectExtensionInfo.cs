using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionInfo
    {
        public Type Type { get; }

        public Dictionary<string, ObjectExtensionPropertyInfo> Properties { get; }

        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionInfo(Type type)
        {
            Type = type;
            Properties = new Dictionary<string, ObjectExtensionPropertyInfo>();
            Configuration = new Dictionary<object, object>();
        }

        public ObjectExtensionPropertyInfo AddProperty(
            string propertyName,
            Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            var propertyInfo = Properties.GetOrAdd(
                propertyName,
                () => new ObjectExtensionPropertyInfo(propertyName)
            );

            configureAction?.Invoke(propertyInfo);

            return propertyInfo;
        }

        public ImmutableList<ObjectExtensionPropertyInfo> GetProperties()
        {
            return Properties.Values.ToImmutableList();
        }
    }
}