using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

/// <summary>
/// This class is equivalent of the <see cref="TransientCachedServiceProvider"/>.
/// Use <see cref="TransientCachedServiceProvider"/> instead of this class, for new projects. 
/// </summary>
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
    
    public virtual T LazyGetRequiredService<T>()
    {
        return (T)LazyGetRequiredService(typeof(T));
    }

    public virtual object LazyGetRequiredService(Type serviceType)
    {
        return this.GetRequiredService(serviceType);
    }

    public virtual T? LazyGetService<T>()
    {
        return (T?)LazyGetService(typeof(T));
    }

    public virtual object? LazyGetService(Type serviceType)
    {
        return GetService(serviceType);
    }

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
}
