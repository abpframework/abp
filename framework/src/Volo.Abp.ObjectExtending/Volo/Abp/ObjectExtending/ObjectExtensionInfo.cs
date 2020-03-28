using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionInfo
    {
        public Type Type { get; }

        protected Dictionary<string, ObjectExtensionPropertyInfo> Properties { get; }

        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionInfo(Type type)
        {
            Type = type;
            Properties = new Dictionary<string, ObjectExtensionPropertyInfo>();
            Configuration = new Dictionary<object, object>();
        }

        public virtual bool HasProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        public virtual ObjectExtensionInfo AddOrUpdateProperty<TProperty>(
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            return AddOrUpdateProperty(
                typeof(TProperty),
                propertyName,
                configureAction
            );
        }

        public virtual ObjectExtensionInfo AddOrUpdateProperty(
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            Check.NotNull(propertyType, nameof(propertyType));
            Check.NotNull(propertyName, nameof(propertyName));

            var propertyInfo = Properties.GetOrAdd(
                propertyName,
                () => new ObjectExtensionPropertyInfo(this, propertyType, propertyName)
            );

            configureAction?.Invoke(propertyInfo);

            return this;
        }

        public virtual ImmutableList<ObjectExtensionPropertyInfo> GetProperties()
        {
            return Properties.Values.ToImmutableList();
        }
    }
}