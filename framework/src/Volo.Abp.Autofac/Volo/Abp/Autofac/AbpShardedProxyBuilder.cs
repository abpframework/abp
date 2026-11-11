using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Volo.Abp.Autofac;

/// <summary>
/// Spreads proxy types over several dynamic modules. Generating a type gets slower as its dynamic module grows, so
/// keeping every proxy of an application in one module makes generating them all grow quadratically with their
/// number. A proxy is routed by the simple name of the type it proxies: Castle names a proxy after that name and keeps
/// names unique only within one module, so types sharing a name must share a shard, or two proxies could get the
/// same full name, which code that caches by a runtime type name relies on. A key therefore always lands in the same
/// shard, and is generated and cached there once.
/// </summary>
public class AbpShardedProxyBuilder : IProxyBuilder
{
    private readonly DefaultProxyBuilder[] _shards;

    // Castle creates a proxy with Activator.CreateInstance, which finds the constructor through the type's reflection
    // cache. The runtime holds that cache weakly, so after a full GC each proxy type built its constructor invoker again
    // and compiled a new invoke stub. Holding the constructors keeps the cache, and the stub, for as long as the type.
    private readonly ConditionalWeakTable<Type, ConstructorInfo[]> _constructors = new();

    public AbpShardedProxyBuilder(int shardCount)
    {
        Check.Positive(shardCount, nameof(shardCount));

        _shards = new DefaultProxyBuilder[shardCount];
        for (var i = 0; i < shardCount; i++)
        {
            _shards[i] = new DefaultProxyBuilder();
        }
    }

    public int ShardCount => _shards.Length;

    public ILogger Logger
    {
        get => _shards[0].Logger;
        set
        {
            foreach (var shard in _shards)
            {
                shard.Logger = value;
            }
        }
    }

    /// <summary>The first shard's scope. Proxy generation does not read it; it exists for <see cref="IProxyBuilder"/>.</summary>
    public ModuleScope ModuleScope => _shards[0].ModuleScope;

    public Type CreateClassProxyType(Type classToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options)
    {
        return RetainConstructors(ShardFor(classToProxy).CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options));
    }

    public Type CreateClassProxyTypeWithTarget(Type classToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options)
    {
        return RetainConstructors(ShardFor(classToProxy).CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options));
    }

    public Type CreateInterfaceProxyTypeWithTarget(Type interfaceToProxy, Type[]? additionalInterfacesToProxy, Type targetType, ProxyGenerationOptions options)
    {
        return RetainConstructors(ShardFor(interfaceToProxy).CreateInterfaceProxyTypeWithTarget(interfaceToProxy, additionalInterfacesToProxy, targetType, options));
    }

    public Type CreateInterfaceProxyTypeWithTargetInterface(Type interfaceToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options)
    {
        return RetainConstructors(ShardFor(interfaceToProxy).CreateInterfaceProxyTypeWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, options));
    }

    public Type CreateInterfaceProxyTypeWithoutTarget(Type interfaceToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options)
    {
        return RetainConstructors(ShardFor(interfaceToProxy).CreateInterfaceProxyTypeWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options));
    }

    private Type RetainConstructors(Type proxyType)
    {
        _constructors.GetValue(proxyType, static type => type.GetConstructors());
        return proxyType;
    }

    private DefaultProxyBuilder ShardFor(Type proxiedType)
    {
        return _shards[(StringComparer.Ordinal.GetHashCode(proxiedType.Name) & int.MaxValue) % _shards.Length];
    }
}
