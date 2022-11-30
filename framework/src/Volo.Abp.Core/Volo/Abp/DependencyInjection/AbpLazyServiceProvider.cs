using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IAbpLazyServiceProvider))]
public class AbpLazyServiceProvider :
    CachedServiceProviderBase,
    IAbpLazyServiceProvider,
    ITransientDependency
{
    public AbpLazyServiceProvider(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public T GetService<T>(T defaultValue)
    {
        return (T)GetService(typeof(T), defaultValue);
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
            serviceType,
            _ => new Lazy<object>(() => factory(ServiceProvider))
        ).Value;
    }

    public virtual T LazyGetRequiredService<T>()
    {
        return (T)LazyGetRequiredService(typeof(T));
    }

    public virtual object LazyGetRequiredService(Type serviceType)
    {
        return this.GetRequiredService(serviceType);
    }

    public virtual T LazyGetService<T>()
    {
        return (T)LazyGetService(typeof(T));
    }

    public virtual object LazyGetService(Type serviceType)
    {
        return GetService(serviceType);
    }

    #region Old Methods

    public virtual T LazyGetService<T>(T defaultValue)
    {
        return GetService(defaultValue);
    }

    public virtual object LazyGetService(Type serviceType, object defaultValue)
    {
        return GetService(serviceType, defaultValue);
    }

    public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory)
    {
        return GetService<T>(factory);
    }

    public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
    {
        return GetService(serviceType, factory);
    }
    
    #endregion
}
