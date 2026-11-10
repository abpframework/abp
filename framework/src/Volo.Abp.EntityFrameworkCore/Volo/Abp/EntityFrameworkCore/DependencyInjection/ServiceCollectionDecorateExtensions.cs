using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection;

internal static class ServiceCollectionDecorateExtensions
{
    internal static IServiceCollection Decorate<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService, TService> decorator)
        where TService : class
    {
        // The container resolves the last registration.
        for (var i = services.Count - 1; i >= 0; i--)
        {
            var descriptor = services[i];
            if (descriptor.ServiceType != typeof(TService) || descriptor.IsKeyedService)
            {
                continue;
            }

            // The same service collection can be decorated more than once, for example when ApplyServices runs again.
            if (descriptor.ImplementationFactory?.Target is Decorator<TService>)
            {
                break;
            }

            // The original registration stays in the container under its own key, so the container still creates and disposes it.
            var innerServiceKey = new object();
            services[i] = new ServiceDescriptor(
                typeof(TService),
                new Decorator<TService>(innerServiceKey, decorator).Create,
                descriptor.Lifetime);
            services.Add(CreateInnerDescriptor(descriptor, innerServiceKey));
            break;
        }

        return services;
    }

    private static ServiceDescriptor CreateInnerDescriptor(ServiceDescriptor descriptor, object innerServiceKey)
    {
        if (descriptor.ImplementationType != null)
        {
            return new ServiceDescriptor(descriptor.ServiceType, innerServiceKey, descriptor.ImplementationType, descriptor.Lifetime);
        }

        if (descriptor.ImplementationFactory != null)
        {
            var implementationFactory = descriptor.ImplementationFactory;
            return new ServiceDescriptor(descriptor.ServiceType, innerServiceKey, (provider, _) => implementationFactory(provider), descriptor.Lifetime);
        }

        return new ServiceDescriptor(descriptor.ServiceType, innerServiceKey, descriptor.ImplementationInstance!);
    }

    private sealed class Decorator<TService>
        where TService : class
    {
        private readonly object _innerServiceKey;
        private readonly Func<IServiceProvider, TService, TService> _decorator;

        public Decorator(object innerServiceKey, Func<IServiceProvider, TService, TService> decorator)
        {
            _innerServiceKey = innerServiceKey;
            _decorator = decorator;
        }

        public object Create(IServiceProvider serviceProvider)
        {
            var inner = serviceProvider.GetRequiredKeyedService<TService>(_innerServiceKey);
            var service = _decorator(serviceProvider, inner);
            if (service == null || ReferenceEquals(service, inner))
            {
                throw new AbpException($"The decorator of {typeof(TService).FullName} must return a new object.");
            }

            return service;
        }
    }
}
