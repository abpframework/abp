## ABP Application Startup

You typically use the [ABP CLI](CLI.md)'s `abp new` command to [get started](Getting-Started.md) with one of the pre-built [startup solution templates](Startup-Templates/Index.md). When you do that, you generally don't need to know the details of how the ABP Framework is integrated with your application or how it is configured and initialized. The startup template also comes with the fundamental ABP packages and [application modules](Modules/Index) are pre-installed and configured for you.

> It is always suggested to [get started with a startup template](Getting-Started.md) and modify it for your requirements. Read this document only if you want to understand the details or if you need to modify how the ABP Framework starts.

While the ABP Framework has a lot of features and integrations, it is built as a lightweight and modular framework. It consists of [hundreds of NuGet and NPM packages](https://abp.io/packages), so you can only use the features you need. If you follow the [Getting Started with an Empty ASP.NET Core MVC / Razor Pages Application](Getting-Started-AspNetCore-Application.md) document, you'll see how easy it is to install the ABP Framework into an empty ASP.NET Core project from scratch. You only need to install a single NuGet package and make a few small changes.

This document is for who wants to better understand how the ABP Framework is initialized and configured on startup.

## Installing to a Console Application

A .NET Console application is the minimalist .NET application. So, it is best to show the installing of the ABP Framework to a console application as a minimalist example.

If you [create a new console application with Visual Studio](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio) (for .NET 7.0 or later), you will see the following solution structure (I named the solution as `MyConsoleDemo`):

![app-startup-console-initial](images/app-startup-console-initial.png)

This example uses the [top level statements](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/top-level-statements), so it consists of only a single line of code.

The first step is to install the [Volo.Abp.Core](https://www.nuget.org/packages/Volo.Abp.Core) NuGet package, which is the most core NuGet package of the ABP framework. You can install it using the [Package Manager Console](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell) in Visual Studio:

````bash
Install-Package Volo.Abp.Core
````

Alternatively, you can use a command-line terminal in the root folder of the project (the folder containing the `MyConsoleDemo.csproj` file, for this example):

````bash
dotnet add package Volo.Abp.Core
````

After adding the NuGet package, we should create a root [module class](Module-Development-Basics.md) for our application. We can create the following class in the project:

````csharp
using Volo.Abp.Modularity;

namespace MyConsoleDemo
{
    public class MyConsoleDemoModule : AbpModule
    {
    }
}
````

This is an empty class deriving from the `AbpModule` class. It is the main class that you will control your application's dependencies with, and implement your configuration and startup/shutdown logic. For more information, please check the [Modularity](Module-Development-Basics.md) document.

As the second and the last step, change the `Program.cs` as shown in the following code block:

````csharp
using MyConsoleDemo;
using Volo.Abp;

// 1: Create the ABP application container
using var application = await AbpApplicationFactory.CreateAsync<MyConsoleDemoModule>();

// 2: Initialize/start the ABP Framework (and all the modules)
await application.InitializeAsync();

Console.WriteLine("ABP Framework has been started...");

// 3: Stop the ABP Framework (and all the modules)
await application.ShutdownAsync();
````

That's all. Now, ABP Framework is installed, integrated, started and stopped in your application. From now, you can install [ABP packages](https://abp.io/packages) to your application whenever you need them.

## Installing a Framework Package

If you want to send emails from your .NET application, you can use .NET's standard [SmtpClient class](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient). ABP also provides an `IEmailSender` service that simplifies [sending emails](Emailing.md) and configuring the email settings in a central place. If you want to use it, you should install the [Volo.Abp.Emailing](https://www.nuget.org/packages/Volo.Abp.Emailing) NuGet package to your project:

````bash
dotnet add package Volo.Abp.Emailing
````

Once you add a new ABP package/module, you also need to specify the module dependency from your module class. So, change the `MyConsoleDemoModule` class as shown below:

````csharp
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace MyConsoleDemo
{
    [DependsOn(typeof(AbpEmailingModule))] // Added the module dependency
    public class MyConsoleDemoModule : AbpModule
    {
    }
}
````

I've just added a `[DependsOn]` attribute to declare that I want to use the ABP Emailing Module (`AbpEmailingModule`). Now, I can use the `IEmailSender` service in my `Program.cs`:

````csharp
using Microsoft.Extensions.DependencyInjection;
using MyConsoleDemo;
using Volo.Abp;
using Volo.Abp.Emailing;

using var application = await AbpApplicationFactory.CreateAsync<MyConsoleDemoModule>();
await application.InitializeAsync();

// Sending emails using the IEmailSender service
var emailsender = application.ServiceProvider.GetRequiredService<IEmailSender>();
await emailsender.SendAsync(
    to: "info@acme.com",
    subject: "Hello World",
    body: "My message body..."
);

await application.ShutdownAsync();
````

> If you run that application, you get a runtime error indicating that the email sending settings haven't been done yet. You can check the [Email Sending document](Emailing.md) to learn how to configure it.

That's all. Install an ABP NuGet package, add the module dependency (using the `[DependsOn]` attribute) and use any service inside the NuGet package.

The [ABP CLI](CLI.md) already has a special command to perform the addition of an ABP NuGet and also adding the `[DependsOn]` attribute to your module class for you with a single command:

````bash
abp add-package Volo.Abp.Emailing
````

We suggest you to use the `abp add-package` command instead of manually doing it.

## AbpApplicationFactory

`AbpApplicationFactory` is the main class that creates an ABP application container. It provides a single static `CreateAsync` (and `Create` if you can't use asynchronous programming) method with multiple overloads. Let's investigate these overloads to understand where you can use them.

The first overload gets a generic module class parameter as we've used before in this document:

````csharp
AbpApplicationFactory.CreateAsync<MyConsoleDemoModule>();
````

The generic class parameter should be the root module class of your application. All the other modules are resolved as dependencies of that module.

The second overload gets the module class as a `Type` parameter, instead of the generic parameter. So, the previous code block could be re-written as shown below:

````csharp
AbpApplicationFactory.CreateAsync(typeof(MyConsoleDemoModule));
````

Both overloads work exactly the same. So, you can use the second one if you don't know the module class type on development time and you (somehow) calculate it on runtime.    

If you create one of the methods above, ABP creates an internal service collection (`IServiceCollection`) and an internal service provider (`IServiceProvider`) to setup the [dependency injection](Dependency-Injection.md) system internally. Notice that we've used the `application.ServiceProvider` property in the *Installing a Framework Package* section to resolve the `IEmailSender` service from the dependency injection system.

The next overload gets an `IServiceCollection` parameter from you to allow you to setup the dependency injection system yourself, or integrate to another framework (like ASP.NET Core) that also sets up the dependency injection system internally.

We can change the `Program.cs` as shown below to externally manage the dependency injection setup:

````csharp
using Microsoft.Extensions.DependencyInjection;
using MyConsoleDemo;
using Volo.Abp;

// 1: Manually created the IServiceCollection
IServiceCollection services = new ServiceCollection();

// 2: Pass the IServiceCollection externally to the ABP Framework
using var application = await AbpApplicationFactory
    .CreateAsync<MyConsoleDemoModule>(services);

// 3: Manually built the IServiceProvider object
IServiceProvider serviceProvider = services.BuildServiceProvider();

// 4: Pass the IServiceProvider externally to the ABP Framework
await application.InitializeAsync(serviceProvider);

Console.WriteLine("ABP Framework has been started...");

await application.ShutdownAsync();
````

In this example, we've used .NET's standard dependency injection container. The `services.BuildServiceProvider()` call creates the standard container. However, ABP provides an alternative extension method, `BuildServiceProviderFromFactory()`, that properly works even if you are using another dependency injection container:

````csharp
IServiceProvider serviceProvider = services.BuildServiceProviderFromFactory();
````

> You can check the [Autofac Integration](Autofac-Integration.md) document if you want to learn how you can integrate the [Autofac](https://autofac.org/) dependency injection container with the ABP Framework.

Finally, the `CreateAsync` method has a last overload that takes the module class name as a `Type` parameter and a `IServiceCollection` object. So, we could re-write the last `CreateAsync` method usage as in the following code block:

````csharp
using var application = await AbpApplicationFactory
    .CreateAsync(typeof(MyConsoleDemoModule), services);
````

> All of the `CreateAsync` method overloads have `Create` counterparts. If your application type can not utilize asynchronous programming (that means you can't use the `await` keyword), then you can use the `Create` method instead of the `CreateAsync` method.

### AbpApplicationCreationOptions

All of the `CreateAsync` overloads can get an optional `Action<AbpApplicationCreationOptions>` parameter to configure the options that are used on the application creation. See the following example:

````csharp
using var application = await AbpApplicationFactory
    .CreateAsync<MyConsoleDemoModule>(options =>
    {
        options.ApplicationName = "MyApp";
    });
````

We've passed a lambda method to configure the `ApplicationName` option. Here's a list of all standard options:

* `ApplicationName`: A human-readable name for the application. It is a unique value for an application.
* `Configuration`: Can be used to setup the [application configuration](Configuration.md) when it is not provided by the hosting system. It is not needed for ASP.NET Core and other .NET hosted applications. However, if you've used `AbpApplicationFactory` with an internal service provider, you can use this option to configure how the application configuration is built.
* `PlugInSources`: A list of plugin sources. See the [Plug-In Modules documentation](PlugIn-Modules) to learn how to work with plugins.
* `Services`: The `IServiceCollection` object that can be used to register service dependencies. You generally don't need that, because you configure your services in your [module class](Module-Development-Basics.md). However, it can be used while writing extension methods for the `AbpApplicationCreationOptions` class.

#### The ApplicationName option

As defined above, the `ApplicationName` option is a human-readable name for the application. It is a unique value for an application.

`ApplicationName` is used by the ABP Framework in several places to distinguish the application. For example, the [audit logging](Audit-Logging.md) system saves the `ApplicationName` in each audit log record written by the related application, so you can understand which application has created the audit log entry. So, if your system consists of multiple applications (like a microservice solution) that are saving audit logs to a single point, you should be sure that each application has a different `ApplicationName`.

The `ApplicationName` property's value is set automatically from the **entry assembly's name** (generally, the project name in a .NET solution) by default, which is proper for most cases, since each application typically has a unique entry assembly name.

There are two ways to set the application name to a different value. In this first approach, you can set the `ApplicationName` property in your application's [configuration](Configuration.md). The easiest way is to add an `ApplicationName` field to your `appsettings.json` file:

````json
{
    "ApplicationName": "Services.Ordering"
}
````

Alternatively, you can set `AbpApplicationCreationOptions.ApplicationName` while creating the ABP application. You can find the `AddApplication` or `AddApplicationAsync` call in your solution (typically in the `Program.cs` file), and set the `ApplicationName` option as shown below:

````csharp
await builder.AddApplicationAsync<OrderingServiceHttpApiHostModule>(options =>
{
    options.ApplicationName = "Services.Ordering";
});
````

#### IApplicationInfoAccessor

If you need to access the `ApplicationName` later in your solution, you can inject the `IApplicationInfoAccessor` service and get the value from its `ApplicationName` property.

`IApplicationInfoAccessor` also provides an `InstanceId` value, that is a random GUID value that is generated when your application starts. You can use that value to distinguish application instances from each other.

## IAbpApplication

`AbpApplicationFactory` returns an `IAbpApplication` object from its `CreateAsync` (or `Create`) method. `IAbpApplication` is the main container for an ABP application. It is also registered to the [dependency injection](Dependency-Injection.md) system, so you can inject `IAbpApplication` in your services to use its properties and methods.

Here's a list of `IAbpApplication` properties you may want to know:

* `StartupModuleType`: Gets the root module of the application that was used while creating the application container (on the `AbpApplicationFactory.CreateAsync` method).
* `Services`: A list of all service registrations (the `IServiceCollection` object). You can not add new services to this collection after application initialization (you can actually add, but it won't have any effect).
* `ServiceProvider`: A reference to the root service provider used by the application. This can not be used before initializing the application. If you need to resolve non-singleton services from that `IServiceProvider` object, always create a new service scope and dispose it after usage. Otherwise, your application will have memory leak problems. See the *Releasing/Disposing Services* section of the [dependency injection](Dependency-Injection.md) document for more information about service scopes.
* `Modules`: A read-only list of all the modules loaded into the current application. Alternatively, you can inject the `IModuleContainer` service if you need to access the module list in your application code.

The `IAbpApplication` interface extends the `IApplicationInfoAccessor` interface, so you can get the `ApplicationName` and `InstanceId` values from it. However, if you only need to access these properties, inject and use the `IApplicationInfoAccessor` service instead.

`IAbpApplication` is disposable. Always dispose of it before exiting your application.

## .NET Generic Host & ASP.NET Core Integrations

`AbpApplicationFactory` can create a standalone ABP application container without any external dependency. However, in most cases, you will want to integrate it with [.NET's generic host](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host) or ASP.NET Core. For such usages, ABP provides built-in extension methods to easily create an ABP application container that is well-integrated to these systems.

The [Getting Started with an Empty ASP.NET Core MVC / Razor Pages Application](Getting-Started-AspNetCore-Application.md) document clearly explains how you can create an ABP application container in an ASP.NET Core application.

You can also [create a console application](Startup-Templates/Console) to see how it is integrated with .NET Generic Host.

> Most of the times, you will directly create ABP applications using the ABP CLI's `new` command. So, you don't need to care about these integration details.

## See Also

* [Dependency injection](Dependency-Injection.md)
* [Modularity](Module-Development-Basics.md)
