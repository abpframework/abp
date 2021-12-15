using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public class ConventionalRegistrar_Tests
{
    public void Should_Use_Custom_Conventions_If_Added()
    {
        //Arrange
        var services = new ServiceCollection();

        //Act
        services.AddConventionalRegistrar(new MyCustomConventionalRegistrar());
        services.AddTypes(typeof(MyCustomClass), typeof(MyClass), typeof(MyNonRegisteredClass));

        //Assert
        services.ShouldContainTransient(typeof(MyClass)); //Registered by default convention.
        services.ShouldContainSingleton(typeof(MyCustomClass)); //Registered by custom convention.
        services.ShouldNotContainService(typeof(MyNonRegisteredClass)); //Not registered by any convention.
    }

    public class MyCustomConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (type == typeof(MyClass))
            {
                services.AddSingleton<MyCustomClass>();
            }
        }
    }

    public class MyCustomClass
    {

    }

    public class MyNonRegisteredClass
    {

    }

    public class MyClass : ITransientDependency
    {

    }
}
