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
            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
        }

        public static void ShouldContainSingleton(this IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
        }
    }
}