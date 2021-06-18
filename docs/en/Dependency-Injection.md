# Dependency Injection

ABP's Dependency Injection system is developed based on Microsoft's [dependency injection extension](https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96) library (Microsoft.Extensions.DependencyInjection nuget package). So, it's documentation is valid in ABP too.

> While ABP has no core dependency to any 3rd-party DI provider, it's required to use a provider that supports dynamic proxying and some other advanced features to make some ABP features properly work. Startup templates come with Autofac installed. See [Autofac integration](Autofac-Integration.md) document for more information.

## Modularity

Since ABP is a modular framework, every module defines it's own services and registers via dependency injection in it's own seperate [module class](Module-Development-Basics.md). Example:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //register dependencies here
    }
}
````

## Conventional Registration

ABP introduces conventional service registration. You need not do anything to register a service by convention. It's automatically done. If you want to disable it, you can set `SkipAutoServiceRegistration` to `true` by overriding the `PreConfigureServices` method.

````C#
public class BlogModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SkipAutoServiceRegistration = true;
    }
}
````

Once you skip auto registration, you should manually register your services. In that case, ``AddAssemblyOf`` extension method can help you to register all your services by convention. Example:

````c#
public class BlogModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SkipAutoServiceRegistration = true;
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAssemblyOf<BlogModule>();
    }
}
````

The section below explains the conventions and configurations.

### Inherently Registered Types

Some specific types are registered to dependency injection by default. Examples:

* Module classes are registered as singleton.
* MVC controllers (inherit ``Controller`` or ``AbpController``) are registered as transient.
* MVC page models (inherit ``PageModel`` or ``AbpPageModel``) are registered as transient.
* MVC view components (inherit ``ViewComponent`` or ``AbpViewComponent``) are registered as transient.
* Application services (inherit ``ApplicationService`` class or its subclasses) are registered as transient.
* Repositories (implement ``BasicRepositoryBase`` class or its subclasses) are registered as transient.
* Domain services (implement ``IDomainService`` interface or inherit ``DomainService`` class) are registered as transient.

Example:

````C#
public class BlogPostAppService : ApplicationService
{
}
````

``BlogPostAppService`` is automatically registered with transient lifetime since it's derived from a known base class.

### Dependency Interfaces

If you implement these interfaces, your class is registered to dependency injection automatically:

* ``ITransientDependency`` to register with transient lifetime.
* ``ISingletonDependency`` to register with singleton lifetime.
* ``IScopedDependency`` to register with scoped lifetime.

Example:

````C#
public class TaxCalculator : ITransientDependency
{
}
````

``TaxCalculator`` is automatically registered with a transient lifetime since it implements ``ITransientDependency``.

### Dependency Attribute

Another way of configuring a service for dependency injection is to use ``DependencyAttribute``. It has the following properties:

* ``Lifetime``: Lifetime of the registration: ``Singleton``, ``Transient`` or ``Scoped``.
* ``TryRegister``: Set ``true`` to register the service only if it's not registered before. Uses TryAdd... extension methods of IServiceCollection.
* ``ReplaceServices``: Set ``true`` to replace services if they are already registered before. Uses Replace extension method of IServiceCollection.

Example:

````C#
[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
public class TaxCalculator
{

}

````

``Dependency`` attribute has a higher priority than other dependency interfaces if it defines the ``Lifetime`` property.

### ExposeServices Attribute 

``ExposeServicesAttribute`` is used to control which services are provided by the related class. Example:

````C#
[ExposeServices(typeof(ITaxCalculator))]
public class TaxCalculator: ICalculator, ITaxCalculator, ICanCalculate, ITransientDependency
{

}
````

``TaxCalculator`` class only exposes ``ITaxCalculator`` interface. That means you can only inject ``ITaxCalculator``, but can not inject ``TaxCalculator`` or ``ICalculator`` in your application.

### Exposed Services by Convention

If you do not specify which services to expose, ABP expose services by convention. So taking the ``TaxCalculator`` defined above:

* The class itself is exposed by default. That means you can inject it by ``TaxCalculator`` class.
* Default interfaces are exposed by default. Default interfaces are determined by naming convention. In this example, ``ICalculator`` and ``ITaxCalculator`` are default interfaces of ``TaxCalculator``, but ``ICanCalculate`` is not.

### Combining All Together

Combining attributes and interfaces is possible as long as it's meaningful.

````C#
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ITaxCalculator))]
public class TaxCalculator : ITaxCalculator, ITransientDependency
{

}
````

### Manually Registering

In some cases, you may need to register a service to the `IServiceCollection` manually, especially if you need to use custom factory methods or singleton instances. In that case, you can directly add services just as [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) describes. Example:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Register an instance as singleton
        context.Services.AddSingleton<TaxCalculator>(new TaxCalculator(taxRatio: 0.18));

        //Register a factory method that resolves from IServiceProvider
        context.Services.AddScoped<ITaxCalculator>(
            sp => sp.GetRequiredService<TaxCalculator>()
        );
    }
}
````

### Replace a Service

If you need to replace an existing service (defined by the ABP framework or another module dependency), you have two options;

1. Use the `Dependency` attribute of the ABP framework as explained above.
2. Use the `IServiceCollection.Replace` method of the Microsoft Dependency Injection library. Example:

````csharp
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Replacing the IConnectionStringResolver service
        context.Services.Replace(ServiceDescriptor.Transient<IConnectionStringResolver, MyConnectionStringResolver>());
    }
}
````

## Injecting Dependencies

There are three common ways of using a service that has already been registered.

### Constructor Injection

This is the most common way of injecting a service into a class. For example:

````C#
public class TaxAppService : ApplicationService
{
    private readonly ITaxCalculator _taxCalculator;

    public TaxAppService(ITaxCalculator taxCalculator)
    {
        _taxCalculator = taxCalculator;
    }

    public void DoSomething()
    {
        //...use _taxCalculator...
    }
}
````

``TaxAppService`` gets ``ITaxCalculator`` in it's constructor. The dependency injection system automatically provides the requested service at runtime.

Constructor injection is preffered way of injecting dependencies to a class. In that way, the class can not be constructed unless all constructor-injected dependencies are provided. Thus, the class explicitly declares it's required services.

### Property Injection

Property injection is not supported by Microsoft Dependency Injection library. However, ABP can integrate with 3rd-party DI providers ([Autofac](https://autofac.org/), for example) to make property injection possible. Example:

````C#
public class MyService : ITransientDependency
{
    public ILogger<MyService> Logger { get; set; }

    public MyService()
    {
        Logger = NullLogger<MyService>.Instance;
    }

    public void DoSomething()
    {
        //...use Logger to write logs...
    }
}
````

For a property-injection dependency, you declare a public property with public setter. This allows the DI framework to set it after creating your class.

Property injected dependencies are generally considered as **optional** dependencies. That means the service can properly work without them. ``Logger`` is such a dependency, ``MyService`` can continue to work without logging.

To make the dependency properly optional, we generally set a default/fallback value to the dependency. In this sample, NullLogger is used as fallback. Thus, ``MyService`` can work but does not write logs if DI framework or you don't set Logger property after creating ``MyService``.

One restriction of property injection is that you cannot use the dependency in your constructor, since it's set after the object construction.

Property injection is also useful when you want to design a base class that has some common services injected by default. If you're going to use constructor injection, all derived classes should also inject depended services into their own constructors which makes development harder. However, be very careful using property injection for non-optional services as it makes it harder to clearly see the requirements of a class.

### Resolve Service from IServiceProvider

You may want to resolve a service directly from ``IServiceProvider``. In that case, you can inject IServiceProvider into your class and use ``GetService`` method as shown below:

````C#
public class MyService : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public MyService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoSomething()
    {
        var taxCalculator = _serviceProvider.GetService<ITaxCalculator>();
        //...
    }
}
````

### Releasing/Disposing Services

If you used a constructor or property injection, you don't need to be concerned about releasing the service's resources. However, if you have resolved a service from ``IServiceProvider``, you might, in some cases, need to take care about releasing the service resources.

ASP.NET Core releases all services at the end of a current HTTP request, even if you directly resolved from ``IServiceProvider`` (assuming you injected IServiceProvider). But, there are several cases where you may want to release/dispose manually resolved services:

* Your code is executed outside of AspNet Core request and the executer hasn't handled the service scope.
* You only have a reference to the root service provider.
* You may want to immediately release & dispose services (for example, you may creating too many services with big memory usage and don't want to overuse memory).

In any case, you can use such a 'using' code block to safely and immediately release services:

````C#
using (var scope = _serviceProvider.CreateScope())
{
    var service1 = scope.ServiceProvider.GetService<IMyService1>();
    var service2 = scope.ServiceProvider.GetService<IMyService2>();
}
````

Both services are released when the created scope is disposed (at the end of the using block).

## Advanced Features

### IServiceCollection.OnRegistred Event

You may want to perform an action for every service registered to the dependency injection. In the `PreConfigureServices` method of your module, register a callback using the `OnRegistred` method as shown below:

````csharp
public class AppModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(ctx =>
        {
            var type = ctx.ImplementationType;
            //...
        });
    }
}
````

`ImplementationType` provides the service type. This callback is generally used to add interceptor to a service. Example:

````csharp
public class AppModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(ctx =>
        {
            if (ctx.ImplementationType.IsDefined(typeof(MyLogAttribute), true))
            {
                ctx.Interceptors.TryAdd<MyLogInterceptor>();
            }
        });
    }
}
````

This example simply checks if the service class has `MyLogAttribute` attribute and adds `MyLogInterceptor` to the interceptor list if so.

> Notice that `OnRegistred` callback might be called multiple times for the same service class if it exposes more than one service/interface. So, it's safe to use `Interceptors.TryAdd` method instead of `Interceptors.Add` method. See [the documentation](Dynamic-Proxying-Interceptors.md) of dynamic proxying / interceptors.

## 3rd-Party Providers

While ABP has no core dependency to any 3rd-party DI provider, it's required to use a provider that supports dynamic proxying and some other advanced features to make some ABP features properly work. 

Startup templates come with Autofac installed. See [Autofac integration](Autofac-Integration.md) document for more information.

## See Also

* [ASP.NET Core Dependency Injection Best Practices, Tips & Tricks](https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96)
