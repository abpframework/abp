using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending;

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
        Action<ObjectExtensionInfo>? configureAction = null)
    {
        return AddOrUpdate(typeof(TObject), configureAction);
    }

    [NotNull]
    public virtual ObjectExtensionManager AddOrUpdate(
        [NotNull] Type[] types,
        Action<ObjectExtensionInfo>? configureAction = null)
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
        Action<ObjectExtensionInfo>? configureAction = null)
    {
        var extensionInfo = ObjectsExtensions.GetOrAdd(
            type,
            _ => new ObjectExtensionInfo(type)
        );

        configureAction?.Invoke(extensionInfo);

        return this;
    }

    public virtual ObjectExtensionInfo? GetOrNull<TObject>()
    {
        return GetOrNull(typeof(TObject));
    }

    public virtual ObjectExtensionInfo? GetOrNull([NotNull] Type type)
    {
        return ObjectsExtensions.GetOrDefault(type);
    }

    [NotNull]
    public virtual ImmutableList<ObjectExtensionInfo> GetExtendedObjects()
    {
        return ObjectsExtensions.Values.ToImmutableList();
    }
}
