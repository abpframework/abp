# Modularity

## Introduction

ABP was designed to support to build fully modular applications and systems where every module may have entities, services, database integration, APIs, UI components and so on;

* This document introduces the basics of the module system.
* [Module development best practice guide](../best-practices) explains some **best practices** to develop **re-usable application modules** based on **DDD** principles and layers. A module designed based on this guide will be **database independent** and can be deployed as a **microservice** if needed.
* [Pre-built application modules](../../../modules) are **ready to use** in any kind of application.
* [Module startup template](../../../solution-templates/application-module) is a jump start way to **create a new module**.
* [ABP CLI](../../../cli/index.md) has commands to support modular development.
* All other framework features are compatible to the modularity system.

## Module Class

Every module should define a module class. The simplest way of defining a module class is to create a class derived from ``AbpModule`` as shown below:

````C#
public class BlogModule : AbpModule
{
            
}
````

### Configuring Dependency Injection & Other Modules

#### ConfigureServices Method

``ConfigureServices`` is the main method to add your services to the dependency injection system and configure other modules. Example:

> These methods have Async versions too, and if you want to make asynchronous calls inside these methods, override the asynchronous versions instead of the synchronous ones.

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //...
    }
}
````

You can register dependencies one by one as stated in Microsoft's [documentation](../../fundamentals/dependency-injection.md). But ABP has a **conventional dependency registration system** which automatically register all services in your assembly. See the [dependency Injection](../../fundamentals/dependency-injection.md) documentation for more about the dependency injection system.

You can also configure other services and modules in this way. Example:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Configure default connection string for the application
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = "......";
        });
    }
}
````

> `ConfigureServices` method has an asynchronous version too: `ConfigureServicesAsync`. If you want to make asynchronous calls (use the `await` keyword) inside this method, override the asynchronous version instead of the synchronous one. If you override both asynchronous and synchronous versions, only the asynchronous version will be executed.

See the [Configuration](../../fundamentals/configuration.md) document for more about the configuration system.

#### Pre & Post Configure Services

``AbpModule`` class also defines ``PreConfigureServices`` and ``PostConfigureServices`` methods to override and write your code just before and just after ``ConfigureServices``. Notice that the code you have written into these methods will be executed before/after the ``ConfigureServices`` methods of all other modules.

> These methods have asynchronous versions too. If you want to make asynchronous calls inside these methods, override the asynchronous versions instead of the synchronous ones.

### Application Initialization

Once all the services of all modules are configured, the application starts by initializing all modules. In this phase, you can resolve services from ``IServiceProvider`` since it's ready and available.

#### OnApplicationInitialization Method

You can override ``OnApplicationInitialization`` method to execute code while application is being started.

**Example:**

````C#
public class BlogModule : AbpModule
{
    public override void OnApplicationInitialization(
        ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        myService.DoSomething();
    }
}
````

`OnApplicationInitialization` method has an asynchronous version too. If you want to make asynchronous calls (use the `await` keyword) inside this method, override the asynchronous version instead of the synchronous one.

**Example:**

````csharp
public class BlogModule : AbpModule
{
    public override Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        await myService.DoSomethingAsync();
    }
}
````

> If you override both asynchronous and synchronous versions, only the asynchronous version will be executed.

``OnApplicationInitialization`` is generally used by the startup module to construct the middleware pipeline for ASP.NET Core applications.

**Example:**

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AppModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMvcWithDefaultRoute();
    }
}
````

You can also perform startup logic if your module requires it

#### Pre & Post Application Initialization

``AbpModule`` class also defines ``OnPreApplicationInitialization`` and ``OnPostApplicationInitialization`` methods to override and write your code just before and just after ``OnApplicationInitialization``. Notice that the code you have written into these methods will be executed before/after the ``OnApplicationInitialization`` methods of all other modules.

> These methods have asynchronous versions too, and if you want to make asynchronous calls inside these methods, override the asynchronous versions instead of the synchronous ones.

### Application Shutdown

Lastly, you can override ``OnApplicationShutdown`` method if you want to execute some code while application is being shutdown.

> This methods has asynchronous version too. If you want to make asynchronous calls inside this method, override the asynchronous version instead of the synchronous one.

## Module Dependencies

In a modular application, it's not unusual for one module to depend upon another module(s). An ABP module must declare a ``[DependsOn]`` attribute if it does have a dependency upon another module, as shown below:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))]
public class BlogModule
{
    //...
}
````

You can use multiple ``DependsOn`` attribute or pass multiple module types to a single ``DependsOn`` attribute depending on your preference.

A depended module may depend on another module, but you only need to define your direct dependencies. ABP investigates the dependency graph for the application at startup and initializes/shutdowns modules in the correct order.

## Additional Module Assemblies

ABP automatically registers all the services of your module to the [dependency injection](../../fundamentals/dependency-injection.md) system. It finds the service types by scanning types in the assembly that defines your module class. That assembly is considered as the main assembly of your module.

Typically, every assembly contains a separate module class definition. Then modules depend on each other using the `DependsOn` attribute as explained in the previous section. However, in some rare cases, your module may consist of multiple assemblies, and only one of them defines a module class, and you want to make the other assemblies parts of your module. In that case, you can use the `AdditionalAssembly` attribute as shown below:

````csharp
[DependsOn(...)] // Your module dependencies as you normally do
[AdditionalAssembly(typeof(BlogService))] // A type in the target assembly
public class BlogModule
{
    //...
}
````

In this example, we assume that the `BlogService` class is inside one assembly (`csproj`) and the `BlogModule` class is inside another assembly (`csproj`). With the `AdditionalAssembly` definition, ABP will load the assembly containing the `BlogService` class as a part of the blog module.

Notice that `BlogService` is only an arbitrary selected type in the target assembly. It is just used to indicate the related assembly. You could use any type in the assembly.

> WARNING: If you need to use the `AdditionalAssembly`, be sure that you don't design your system in a wrong way. With this example above, the `BlogService` class' assembly should normally have its own module class and the `BlogModule` should depend on it using the `DependsOn` attribute. Do not use the `AdditionalAssembly` attribute when you can already use the `DependsOn` attribute.

## Framework Modules vs Application Modules

There are **two types of modules.** They don't have any structural difference but categorized by functionality and purpose:

- **Framework modules**: These are **core modules of the framework** like caching, emailing, theming, security, serialization, validation, EF Core integration, MongoDB integration... etc. They do not have application/business functionalities but makes your daily development easier by providing common infrastructure, integration and abstractions.
- **Application modules**: These modules implement **specific application/business functionalities** like blogging, document management, identity management, tenant management... etc. They generally have their own entities, services, APIs and UI components. See [pre-built application modules](../../../modules).

## See Also
* [Video tutorial](https://abp.io/video-courses/essentials/modularity)
