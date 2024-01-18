using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public abstract class CachedServiceProviderBase : ICachedServiceProviderBase
{
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<CachedServiceDescriptor, Lazy<object?>> CachedServices { get; }

    protected CachedServiceProviderBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedServices = new ConcurrentDictionary<CachedServiceDescriptor, Lazy<object?>>();
        CachedServices.TryAdd(new CachedServiceDescriptor(null, typeof(IServiceProvider)), new Lazy<object?>(() => ServiceProvider));
    }

    public virtual object? GetService(Type serviceType)
    {
        return CachedServices.GetOrAdd(
            new CachedServiceDescriptor(null, serviceType),
            _ => new Lazy<object?>(() => ServiceProvider.GetService(serviceType))
        ).Value;
    }

    public T GetService<T>(T defaultValue)
    {
        return (T)GetService(typeof(T), defaultValue!);
    }

    public object GetService(Type serviceType, object defaultValue)
    {
        return GetService(serviceType) ?? defaultValue;
    }

    public T GetService<T>(Func<IServiceProvider, object> factory)
    {
        return (T)GetService(typeof(T), factory);
    }

    public object GetService(Type serviceType, Func<IServiceProvider, object> factory)
    {
        return CachedServices.GetOrAdd(
            new CachedServiceDescriptor(null, serviceType),
            _ => new Lazy<object?>(() => factory(ServiceProvider))
        ).Value!;
    }

    public virtual T GetKeyedService<T>(object? serviceKey)
    {
        return (T)GetKeyedService(typeof(T), serviceKey)!;
    }

    public virtual object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (ServiceProvider is IKeyedServiceProvider requiredServiceSupportingProvider)
        {
            return CachedServices.GetOrAdd(
                new CachedServiceDescriptor(serviceKey, serviceType),
                _ => new Lazy<object?>(() =>  requiredServiceSupportingProvider.GetKeyedService(serviceType, serviceKey))
            ).Value;
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }

    public virtual T GetRequiredKeyedService<T>(object? serviceKey)
    {
        return (T)GetRequiredKeyedService(typeof(T), serviceKey);
    }

    public virtual object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (ServiceProvider is IKeyedServiceProvider requiredServiceSupportingProvider)
        {
            return CachedServices.GetOrAdd(
                new CachedServiceDescriptor(serviceKey, serviceType),
                _ => new Lazy<object?>(() =>  requiredServiceSupportingProvider.GetRequiredKeyedService(serviceType, serviceKey))
            ).Value!;
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }
}
