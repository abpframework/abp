using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionInfo
    {
        [NotNull]
        public Type Type { get; }

        [NotNull]
        protected ConcurrentDictionary<string, ObjectExtensionPropertyInfo> Properties { get; }

        [NotNull]
        public ConcurrentDictionary<object, object> Configuration { get; }

        [NotNull]
        public List<Action<ObjectExtensionValidationContext>> Validators { get; }

        public ObjectExtensionInfo([NotNull] Type type)
        {
            Type = Check.NotNull(type, nameof(type));
            Properties = new ConcurrentDictionary<string, ObjectExtensionPropertyInfo>();
            Configuration = new ConcurrentDictionary<object, object>();
            Validators = new List<Action<ObjectExtensionValidationContext>>();
        }

        public virtual bool HasProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        [NotNull]
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

        [NotNull]
        public virtual ObjectExtensionInfo AddOrUpdateProperty(
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ObjectExtensionPropertyInfo> configureAction = null)
        {
            Check.NotNull(propertyType, nameof(propertyType));
            Check.NotNull(propertyName, nameof(propertyName));

            var propertyInfo = Properties.GetOrAdd(
                propertyName,
                _ => new ObjectExtensionPropertyInfo(this, propertyType, propertyName)
            );

            configureAction?.Invoke(propertyInfo);

            return this;
        }

        [NotNull]
        public virtual ImmutableList<ObjectExtensionPropertyInfo> GetProperties()
        {
            return Properties.OrderBy(t=>t.Key)
                            .Select(t=>t.Value)
                            .ToImmutableList();
        }

        [CanBeNull]
        public virtual ObjectExtensionPropertyInfo GetPropertyOrNull(
            [NotNull] string propertyName)
        {
            Check.NotNullOrEmpty(propertyName, nameof(propertyName));

            return Properties.GetOrDefault(propertyName);
        }
    }
}
