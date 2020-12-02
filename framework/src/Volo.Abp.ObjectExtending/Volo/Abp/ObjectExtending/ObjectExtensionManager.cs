using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionManager
    {
        public static ObjectExtensionManager Instance { get; protected set; } = new ObjectExtensionManager();

        [NotNull]
        public ConcurrentDictionary<object, object> Configuration { get; }

        protected ConcurrentDictionary<Type, ObjectExtensionInfo> ObjectsExtensions { get; }

        protected internal ObjectExtensionManager()
        {
            ObjectsExtensions = new ConcurrentDictionary<Type, ObjectExtensionInfo>();
            Configuration = new ConcurrentDictionary<object, object>();
        }

        [NotNull]
        public virtual ObjectExtensionManager AddOrUpdate<TObject>(
            [CanBeNull] Action<ObjectExtensionInfo> configureAction = null)
            where TObject : IHasExtraProperties
        {
            return AddOrUpdate(typeof(TObject), configureAction);
        }

        [NotNull]
        public virtual ObjectExtensionManager AddOrUpdate(
            [NotNull] Type[] types,
            [CanBeNull] Action<ObjectExtensionInfo> configureAction = null)
        {
            Check.NotNull(types, nameof(types));

            foreach (var type in types)
            {
                AddOrUpdate(type, configureAction);
            }

            return this;
        }

        [NotNull]
        public virtual ObjectExtensionManager AddOrUpdate(
            [NotNull] Type type,
            [CanBeNull] Action<ObjectExtensionInfo> configureAction = null)
        {
            Check.AssignableTo<IHasExtraProperties>(type, nameof(type));

            var extensionInfo = ObjectsExtensions.GetOrAdd(
                type,
                _ => new ObjectExtensionInfo(type)
            );

            configureAction?.Invoke(extensionInfo);

            return this;
        }

        [CanBeNull]
        public virtual ObjectExtensionInfo GetOrNull<TObject>()
            where TObject : IHasExtraProperties
        {
            return GetOrNull(typeof(TObject));
        }

        [CanBeNull]
        public virtual ObjectExtensionInfo GetOrNull([NotNull] Type type)
        {
            Check.AssignableTo<IHasExtraProperties>(type, nameof(type));

            return ObjectsExtensions.GetOrDefault(type);
        }

        [NotNull]
        public virtual ImmutableList<ObjectExtensionInfo> GetExtendedObjects()
        {
            return ObjectsExtensions.Values.ToImmutableList();
        }
    }
}
