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
        public void Should_Register_For_Exposed_Services()
        {
            _services.AddType(typeof(MyServiceWithExposeList));

            _services.ShouldContain(typeof(IMyService1), typeof(MyServiceWithExposeList), ServiceLifetime.Transient);
            _services.ShouldContain(typeof(IMyService2), typeof(MyServiceWithExposeList), ServiceLifetime.Transient);
            _services.ShouldNotContain(typeof(MyServiceWithExposeList));
        }

        [Fact]
        public void AddObjectAccessor_Test()
        {
            //Arrange

            var obj = new MyEmptyClass();

            //Act

            var accessor = _services.AddObjectAccessor<MyEmptyClass>();
            accessor.Object = obj;

            //Assert

            _services.GetSingletonInstance<IObjectAccessor<MyEmptyClass>>().Object.ShouldBe(obj);
            _services.GetSingletonInstance<ObjectAccessor<MyEmptyClass>>().Object.ShouldBe(obj);

            var serviceProvider = _services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IObjectAccessor<MyEmptyClass>>().Object.ShouldBe(obj);
            serviceProvider.GetRequiredService<ObjectAccessor<MyEmptyClass>>().Object.ShouldBe(obj);
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

        public class MyEmptyClass
        {
            
        }
    }
}
