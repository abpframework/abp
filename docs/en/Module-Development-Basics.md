# Modularity

## Introduction

ABP Framework was designed to support to build fully modular applications and systems where every module may have entities, services, database integration, APIs, UI components and so on;

* This document introduces the basics of the module system.
* [Module development best practice guide](Best-Practices/Index.md) explains some **best practices** to develop **re-usable application modules** based on **DDD** principles and layers. A module designed based on this guide will be **database independent** and can be deployed as a **microservice** if needed.
* [Pre-built application modules](Modules/Index.md) are **ready to use** in any kind of application.
* [Module startup template](Startup-Templates/Module.md) is a jump start way to **create a new module**.
* [ABP CLI](CLI.md) has commands to support modular development.
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

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //...
    }
}
````

You can register dependencies one by one as stated in Microsoft's [documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection). But ABP has a **conventional dependency registration system** which automatically register all services in your assembly. See the [dependency Injection](Dependency-Injection.md) documentation for more about the dependency injection system.

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

See the [Configuration](Configuration.md) document for more about the configuration system.

#### Pre & Post Configure Services

``AbpModule`` class also defines ``PreConfigureServices`` and ``PostConfigureServices`` methods to override and write your code just before and just after ``ConfigureServices``. Notice that the code you have written into these methods will be executed before/after the ``ConfigureServices`` methods of all other modules.

### Application Initialization

Once all the services of all modules are configured, the application starts by initializing all modules. In this phase, you can resolve services from ``IServiceProvider`` since it's ready and available.

#### OnApplicationInitialization Method

You can override ``OnApplicationInitialization`` method to execute code while application is being started. Example:

````C#
public class BlogModule : AbpModule
{
    //...

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        myService.DoSomething();
    }
}
````

``OnApplicationInitialization`` is generally used by the startup module to construct the middleware pipeline for ASP.NET Core applications. Example:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AppModule : AbpModule
{
    //...

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

### Application Shutdown

Lastly, you can override ``OnApplicationShutdown`` method if you want to execute some code while application is being shutdown.

## Module Dependencies

In a modular application, it's not unusual for one module to depend upon another module(s). An Abp module must declare ``[DependsOn]`` attribute if it does have a dependency upon another module, as shown below:

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

## Framework Modules vs Application Modules

There are **two types of modules.** They don't have any structural difference but categorized by functionality and purpose:

- **Framework modules**: These are **core modules of the framework** like caching, emailing, theming, security, serialization, validation, EF Core integration, MongoDB integration... etc. They do not have application/business functionalities but makes your daily development easier by providing common infrastructure, integration and abstractions.
- **Application modules**: These modules implement **specific application/business functionalities** like blogging, document management, identity management, tenant management... etc. They generally have their own entities, services, APIs and UI components. See [pre-built application modules](Modules/Index.md).
