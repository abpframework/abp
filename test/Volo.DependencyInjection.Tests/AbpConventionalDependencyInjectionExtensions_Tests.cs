using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.DependencyInjection.Tests
{
    public class AbpConventionalDependencyInjectionExtensions_Tests
    {
        private readonly IServiceCollection _services;

        public AbpConventionalDependencyInjectionExtensions_Tests()
        {
            _services = new ServiceCollection();
        }
        
        [Fact]
        public void Should_Register_Transient()
        {
            //Act
            _services.AddType(typeof(MyTransientClass));

            //Assert
            ShouldContainTransient(_services, typeof(MyTransientClass));
        }

        [Fact]
        public void Should_Register_Singleton()
        {
            //Act
            _services.AddType(typeof(MySingletonClass));

            //Assert
            ShouldContainSingleton(_services, typeof(MySingletonClass));
        }

        private static void ShouldContainTransient(IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Transient);
        }

        private static void ShouldContainSingleton(IServiceCollection services, Type type)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == type);

            serviceDescriptor.ImplementationType.ShouldBe(type);
            serviceDescriptor.ShouldNotBeNull();
            serviceDescriptor.ImplementationFactory.ShouldBeNull();
            serviceDescriptor.ImplementationInstance.ShouldBeNull();
            serviceDescriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
        }

        public class MyTransientClass : ITransientDependency
        {
            
        }

        public class MySingletonClass : ISingletonDependency
        {

        }
    }
}
