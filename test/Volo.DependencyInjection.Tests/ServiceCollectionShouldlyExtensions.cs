using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Volo.DependencyInjection.Tests
{
    public static class ServiceCollectionShouldlyExtensions
    {
        public static void ShouldContainTransient(this IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
        }

        public static void ShouldContainSingleton(this IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
        }

        public static void ShouldContainScoped(this IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Scoped);
        }

        public static void ShouldContain(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(implementationType);
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(lifetime);
        }

        public static void ShouldNotContain(this IServiceCollection services, Type serviceType)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);

            serviceDescriptor.ShouldBeNull();
        }
    }
}