using Microsoft.Extensions.DependencyInjection;
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
        
        public class MyTransientClass : ITransientDependency
        {
            
        }

        public class MySingletonClass : ISingletonDependency
        {

        }
    }
}
