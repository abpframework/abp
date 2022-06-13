using System;
using System.Collections.Concurrent;

namespace Volo.Abp.DependencyInjection;

public abstract class CachedServiceProviderBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, Lazy<object>> _cachedServices;

    protected CachedServiceProviderBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cachedServices = new ConcurrentDictionary<Type, Lazy<object>>();
        _cachedServices.TryAdd(typeof(IServiceProvider), new Lazy<object>(() => _serviceProvider));
    }

    public virtual object GetService(Type serviceType)
    {
        return _cachedServices.GetOrAdd(
            serviceType,
            _ => new Lazy<object>(() => _serviceProvider.GetService(serviceType))
        ).Value;
    }
}