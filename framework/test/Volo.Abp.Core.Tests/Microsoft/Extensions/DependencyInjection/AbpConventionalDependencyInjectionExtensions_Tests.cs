using System.Linq;
using Shouldly;
using Xunit;
using Volo.Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
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
            _services.ShouldContainTransient(typeof(MyTransientClass));
        }

        [Fact]
        public void Should_Register_Singleton()
        {
            //Act
            _services.AddType(typeof(MySingletonClass));

            //Assert
            _services.ShouldContainSingleton(typeof(MySingletonClass));
        }

        [Fact]
        public void Should_Register_Scoped()
        {
            //Act
            _services.AddType(typeof(MyScopedClass));

            //Assert
            _services.ShouldContainScoped(typeof(MyScopedClass));
        }

        [Fact]
        public void Should_Register_Scoped_With_Dependency_Attribute()
        {
            //Act
            _services.AddType(typeof(MyScopedClassWithDependencyAttribute));

            //Assert
            _services.ShouldContainScoped(typeof(MyScopedClassWithDependencyAttribute));
        }

        [Fact]
        public void Dependency_Attribute_Should_Override_Interface_Lifetimes()
        {
            //Act
            _services.AddType(typeof(MyScopedClassWithDependencyAttribute2));

            //Assert
            _services.ShouldContainScoped(typeof(MyScopedClassWithDependencyAttribute2));
        }

        [Fact]
        public void Should_Register_For_Exposed_Services()
        {
            //Act
            _services.AddType(typeof(MyServiceWithExposeList));

            //Assert
            _services.ShouldContain(typeof(IMyService1), typeof(MyServiceWithExposeList), ServiceLifetime.Transient);
            _services.ShouldContain(typeof(IMyService2), typeof(MyServiceWithExposeList), ServiceLifetime.Transient);
            _services.ShouldNotContainService(typeof(MyServiceWithExposeList));
        }

        [Fact]
        public void Should_Register_Multiple_Implementation_For_Same_Service()
        {
            //Act

            _services.AddTypes(typeof(FirstImplOfMyService), typeof(SecondImplOfMyService));

            //Assert

            //Check descriptions in service collection
            var descriptions = _services.Where(s => s.ServiceType == typeof(IMyService)).ToList();
            descriptions.Count.ShouldBe(2);
            descriptions[0].ImplementationType.ShouldBe(typeof(FirstImplOfMyService));
            descriptions[1].ImplementationType.ShouldBe(typeof(SecondImplOfMyService));

            //Check from service provider
            var serviceProvider = _services.BuildServiceProvider();

            //Default service should be second one
            serviceProvider.GetRequiredService<IMyService>().ShouldBeOfType(typeof(SecondImplOfMyService));

            //Should also get all services
            var instances = serviceProvider.GetServices<IMyService>().ToList();
            instances.Count.ShouldBe(2);
            instances[0].ShouldBeOfType(typeof(FirstImplOfMyService));
            instances[1].ShouldBeOfType(typeof(SecondImplOfMyService));
        }

        [Fact]
        public void Should_Not_Register_Second_Implementation_For_Same_Service_If_Second_Is_Marked_As_TryRegister()
        {
            //Act

            _services.AddTypes(typeof(FirstImplOfMyService), typeof(TryRegisterImplOfMyService));

            //Assert

            //Check descriptions in service collection
            var descriptions = _services.Where(s => s.ServiceType == typeof(IMyService)).ToList();
            descriptions.Count.ShouldBe(1);
            descriptions[0].ImplementationType.ShouldBe(typeof(FirstImplOfMyService));

            //Check from service provider
            var serviceProvider = _services.BuildServiceProvider();

            //Default service should be second one
            serviceProvider.GetRequiredService<IMyService>().ShouldBeOfType(typeof(FirstImplOfMyService));

            //Should also get all services
            var instances = serviceProvider.GetServices<IMyService>().ToList();
            instances.Count.ShouldBe(1);
            instances[0].ShouldBeOfType(typeof(FirstImplOfMyService));
        }

        [Fact]
        public void Should_Replace_First_Implementation_By_Second_If_Second_Marked_As_ReplaceServices()
        {
            //Act

            _services.AddTypes(typeof(FirstImplOfMyService), typeof(MyServiceReplacesIMyService));

            //Assert

            //Check descriptions in service collection
            var descriptions = _services.Where(s => s.ServiceType == typeof(IMyService)).ToList();
            descriptions.Count.ShouldBe(1);
            descriptions[0].ImplementationType.ShouldBe(typeof(MyServiceReplacesIMyService));

            //Check from service provider
            var serviceProvider = _services.BuildServiceProvider();

            //Default service should be second one
            serviceProvider.GetRequiredService<IMyService>().ShouldBeOfType(typeof(MyServiceReplacesIMyService));

            //Should also get all services
            var instances = serviceProvider.GetServices<IMyService>().ToList();
            instances.Count.ShouldBe(1);
            instances[0].ShouldBeOfType(typeof(MyServiceReplacesIMyService));
        }

        [Fact]
        public void Should_Not_Register_Classes_Marked_With_DisableConventionalRegistration()
        {
            _services.AddType(typeof(NonConventionalImplOfMyService));

            _services.ShouldNotContainService(typeof(IMyService));
            _services.ShouldNotContainService(typeof(NonConventionalImplOfMyService));
        }

        [Fact]
        public void AddObjectAccessor_Test()
        {
            //Arrange

            var obj = new MyEmptyClass();

            //Act

            var accessor = _services.AddObjectAccessor<MyEmptyClass>();
            accessor.Value = obj;

            //Assert

            _services.GetSingletonInstance<IObjectAccessor<MyEmptyClass>>().Value.ShouldBe(obj);
            _services.GetSingletonInstance<ObjectAccessor<MyEmptyClass>>().Value.ShouldBe(obj);

            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IObjectAccessor<MyEmptyClass>>().Value.ShouldBe(obj);
            serviceProvider.GetRequiredService<ObjectAccessor<MyEmptyClass>>().Value.ShouldBe(obj);
        }

        public class MyTransientClass : ITransientDependency
        {
            
        }

        public class MySingletonClass : ISingletonDependency
        {

        }

        public class MyScopedClass : IScopedDependency
        {

        }

        [Dependency(ServiceLifetime.Scoped)]
        public class MyScopedClassWithDependencyAttribute
        {

        }

        [Dependency(ServiceLifetime.Scoped)] //Attribute overrides interface
        public class MyScopedClassWithDependencyAttribute2 : ITransientDependency
        {

        }

        public interface IMyService1
        {
            
        }

        public interface IMyService2
        {

        }
        
        [ExposeServices(typeof(IMyService1), typeof(IMyService2))]
        public class MyServiceWithExposeList : IMyService1, IMyService2, ITransientDependency
        {
            
        }
        
        [Dependency(ReplaceServices = true)]
        public class MyServiceReplacesIMyService : IMyService
        {
            
        }

        public interface IMyService : ITransientDependency
        {
            
        }

        public class FirstImplOfMyService : IMyService
        {
        }

        public class SecondImplOfMyService : IMyService
        {
        }

        [Dependency(TryRegister = true)]
        public class TryRegisterImplOfMyService : IMyService
        {
        }

        [DisableConventionalRegistration]
        public class NonConventionalImplOfMyService : IMyService
        {
            
        }

        public class MyEmptyClass
        {
            
        }
    }
}
