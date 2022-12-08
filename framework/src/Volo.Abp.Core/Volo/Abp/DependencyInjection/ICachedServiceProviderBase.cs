using System;

namespace Volo.Abp.DependencyInjection;

public interface ICachedServiceProviderBase : IServiceProvider
{
    T GetService<T>(T defaultValue);
    
    object GetService(Type serviceType, object defaultValue);

    T GetService<T>(Func<IServiceProvider, object> factory);

    object GetService(Type serviceType, Func<IServiceProvider, object> factory);
}