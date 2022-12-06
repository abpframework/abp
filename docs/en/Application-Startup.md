## ABP Application Startup

You typically use the [ABP CLI](CLI.md)'s `abp new` command to [get started](Getting-Started.md) with one of the pre-built [startup solution templates](Startup-Templates/Index.md). When you do that, you generally don't need to know the details of how the ABP Framework is integrated with your application, how it is configured and initialized. The startup template comes with many fundamental ABP packages and [application modules](Modules/Index) are pre-installed and configured for you.

> It is always suggested to [get started with a startup template](Getting-Started.md) and modify it for your requirements. Read that document only if you want to understand the details or you need to modify how the application starts.

While the ABP Framework has a lot of features and integrations, it is built as a lightweight and modular framework. It consists of hundreds of NuGet and NMP packages, so you can use only the features you need to. If you follow the [Getting Started with an Empty ASP.NET Core MVC / Razor Pages Application](Getting-Started-AspNetCore-Application.md) document, you see how easy to install the ABP Framework into an empty ASP.NET Core project from scratch. You only install a single NuGet package and make a few small changes.

This document is for who want to better understand how the ABP Framework is initialized and configured on startup.

## Installing to a Console Application

A .NET Console application is the minimalist .NET application. So, it is best to show installing the ABP Framework to a console application as a minimalist example.

If you [create a new console application with Visual Studio](https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio) (for .NET 7.0 or later), you will see the following solution structure (I named the solution as `MyConsoleDemo`):

![app-startup-console-initial](images/app-startup-console-initial.png)

This example uses the [top level statements](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/top-level-statements), so it consists of only a single line of code. The first step is to install the [Volo.Abp.Core](https://www.nuget.org/packages/Volo.Abp.Core) NuGet package, which is the most core NuGet package of the ABP framework. You can install it using [Package Manager Console](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell) in Visual Studio:

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

This is an empty class deriving from the `AbpModule` class. It is the main class that you will control your application's dependencies and implement your configuration, startup and shutdown logic. For more information, please check the [Modularity](Module-Development-Basics.md) document.

As the second and the last step, change the `Program.cs` as shown in the following code block:

````csharp
using MyConsoleDemo;
using Volo.Abp;

// 1: Create the application container
using var application = await AbpApplicationFactory.CreateAsync<MyConsoleDemoModule>();

// 2: Initialize/start the ABP Framework and all the modules
await application.InitializeAsync();

Console.WriteLine("ABP Framework has been started...");

// 3: Stop the ABP Framework and all the modules
await application.ShutdownAsync();
````

That's all. Now, ABP Framework is installed, integrated, started and stopped in your application. From now, you can install [ABP packages](https://abp.io/packages) to your application whenever you need them.

## Installing a Framework Package

For example, if you want to send emails from your application, you can use .NET's standard [SmtpClient class](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient). ABP also provides an `IEmailSender` service that simplifies [sending emails](Emailing.md) and configuring the email settings in a central place. If you want to use it, you should install the [Volo.Abp.Emailing](https://www.nuget.org/packages/Volo.Abp.Emailing) NuGet package to your project:

````bash
dotnet add package Volo.Abp.Emailing
````

Once you add a new ABP package/module, you also need to specify the module dependency from your module class. So, change the `MyConsoleDemoModule` class as shown below:

````csharp
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace MyConsoleDemo
{
    [DependsOn(typeof(AbpEmailingModule))]
    public class MyConsoleDemoModule : AbpModule
    {
    }
}
````

I've just added a `DependsOn` attribute to declare that I want to use the ABP Emailing Module (`AbpEmailingModule`). Now, I can use the `IEmailSender` service in my `Program.cs`:

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

> If you run this application, you get a runtime error indicating that the email sending settings hasn't been done yet. You can check the [Email Sending](Emailing.md) document to learn how to configure it.

That's all. Install an ABP NuGet package, add the module dependency (using the `DependsOn` attribute) and use any service inside the NuGet package.

The [ABP CLI](CLI.md) already has a special command to perform adding an ABP NuGet and also adding the `DependsOn` attribute to your module class for you with a single command:

````bash
abp add-package Volo.Abp.Emailing
````

We suggest you to use the `add-package` command instead of manually doing it.

## AbpApplicationFactory

`AbpApplicationFactory` is the main class that creates an ABP application container. It provides a single `CreateAsync` (and `Create` if you can't use asynchronous programming) method with multiple overloads. Let's investigate these overloads to understand where you can use them.

The first overload gets a generic module class parameter as we've used before in this document:

````csharp
AbpApplicationFactory.CreateAsync<MyConsoleDemoModule>();
````

The second overload gets the module class as a `Type` parameter, instead of a generic parameter. So, the previous code block can be re-written as shown below:

````csharp
AbpApplicationFactory.CreateAsync(typeof(MyConsoleDemoModule));
````

Both overloads works exactly same. So, you can use the second one if you don't know the module class type on development time and you (somehow) calculate it on runtime.    

If you create one of the methods above, ABP creates an internal service collection (`IServiceCollection`) and an internal service provider (`IServiceProvider`) to automate the [dependency injection](Dependency-Injection.md) setup from your application code. Notice that we've used the `application.ServiceProvider` property in the *Installing a Framework Package* section to resolve the `IEmailSender` service from the dependency injection system.

The next overload gets an `IServiceCollection` parameter from you to allow you to set up the dependency injection system yourself, or integrate to another framework (like ASP.NET Core) that also setups the dependency injection system internally.

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

In this example, we've used .NET's standard dependency injection container. The `services.BuildServiceProvider()` call creates the standard container. However, ABP provides an alternative extension method, `BuildServiceProviderFromFactory()`, that gracefully work even if you are using another DI container:

````csharp
IServiceProvider serviceProvider = services.BuildServiceProviderFromFactory();
````

You can check the [Autofac Integration](Autofac-Integration.md) document if you want to learn how you can integrate the [Autofac](https://autofac.org/) dependency injection container with the ABP Framework.

Finally, the `CreateAsync` method has a last overload that takes the module class name as a `Type` parameter and a `IServiceCollection` object. So, we could re-write the last `CreateAsync` method usage as like in the following code block:

````csharp
using var application = await AbpApplicationFactory
    .CreateAsync(typeof(MyConsoleDemoModule), services);
````

> All of the `CreateAsync` method overloads have `Create` counterparts. If your application type can not utilize asynchronous programming (that means you can't use the `await` keyword), then you can use the `Create` method instead of the `CreateAsync` method.

### AbpApplicationCreationOptions

All of the `CreateAsync` overloads can get an optional `Action<AbpApplicationCreationOptions>` parameter to configure the options that is used on the application creation. See the following example:

````csharp
using var application = await AbpApplicationFactory
    .CreateAsync<MyConsoleDemoModule>(options =>
    {
        options.ApplicationName = "MyApp";
    });
````

We've passed a lambda method to configure the `ApplicationName` option.