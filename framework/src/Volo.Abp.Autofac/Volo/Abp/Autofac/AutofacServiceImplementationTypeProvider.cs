using System;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Autofac;

public class AutofacServiceImplementationTypeProvider : IServiceImplementationTypeProvider, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public AutofacServiceImplementationTypeProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual Type? GetImplementationTypeOrNull(Type serviceType)
    {
        if (ServiceProvider is not AutofacServiceProvider autofacServiceProvider)
        {
            return null;
        }

        var registry = autofacServiceProvider.LifetimeScope.ComponentRegistry;
        var service = new TypedService(serviceType);
        if (!registry.TryGetServiceRegistration(service, out var serviceRegistration))
        {
            return null;
        }

        // A decorator or other service middleware may return another object than the registration creates.
        if (registry.ServiceMiddlewareFor(service).Any())
        {
            return null;
        }

        // The reflection activator always creates its type (a class proxy, when the service is intercepted);
        // a delegate may create any type.
        return serviceRegistration.Registration.Activator is ReflectionActivator activator
            ? activator.LimitType
            : null;
    }
}
