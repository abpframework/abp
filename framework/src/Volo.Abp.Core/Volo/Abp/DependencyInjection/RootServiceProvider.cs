using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IRootServiceProvider))]
public class RootServiceProvider : IRootServiceProvider, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public RootServiceProvider(IObjectAccessor<IServiceProvider> objectAccessor)
    {
        ServiceProvider = objectAccessor.Value!;
    }

    public virtual object? GetService(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }

    public virtual object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (ServiceProvider is IKeyedServiceProvider requiredServiceSupportingProvider)
        {
            return requiredServiceSupportingProvider.GetKeyedService(serviceType, serviceKey);
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }

    public virtual object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (ServiceProvider is IKeyedServiceProvider requiredServiceSupportingProvider)
        {
            return requiredServiceSupportingProvider.GetRequiredKeyedService(serviceType, serviceKey);
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }
}
