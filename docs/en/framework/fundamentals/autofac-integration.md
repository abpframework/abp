# Autofac Integration

[Autofac](https://autofac.org/) is one of the most used dependency injection frameworks for .NET. It provides advanced features compared to .Net Core's standard DI library, like dynamic proxying and property injection.

## Install Autofac Integration

> All the [startup templates](../../get-started) and samples are Autofac integrated. So, most of the time you don't need to manually install this package.

If you're not using a startup template, you can use the [ABP CLI](../../cli) to install it to your project. Execute the following command in the folder that contains the .csproj file of your project (suggested to add it to the executable/web project):

````bash
abp add-package Volo.Abp.Autofac
````

> If you haven't done it yet, you first need to install the [ABP CLI](../../cli). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.Autofac).
>

Finally, configure `AbpApplicationCreationOptions` to replace default dependency injection services by Autofac. It depends on the application type.

### ASP.NET Core Application

Call `UseAutofac()` in the **Program.cs** file as shown below:

````csharp
public class Program
{
    public static int Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseAutofac(); //Integrate Autofac!
}
````

If you are using the static `WebApplication` class, you can call the `UseAutofac()` extension method as shown below:

````csharp
public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseAutofac(); // Integrate Autofac!
        await builder.AddApplicationAsync<MyProjectNameWebModule>();
        var app = builder.Build();
        await app.InitializeApplicationAsync();
        await app.RunAsync();
    }
}
````

### Console Application

Call `UseAutofac()` method in the `AbpApplicationFactory.Create` options as shown below:

````csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace AbpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AppModule>(options =>
            {
                options.UseAutofac(); //Autofac integration
            }))
            {
                //...
            }
        }
    }
}
````

## Using the Autofac Registration API

If you want to use Autofac's advanced [registration API](https://autofac.readthedocs.io/en/latest/register/registration.html), you need to access the `ContainerBuilder` object. [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) nuget package defines the `IServiceCollection.GetContainerBuilder()` extension method to obtain the `ContainerBuilder` object.

**Example: Get the `ContainerBuilder` object in the `ConfigureServices` method of your [module class](../architecture/modularity/basics.md)**

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var containerBuilder = context.Services.GetContainerBuilder();
    containerBuilder.RegisterType<MyService>(); // Using Autofac's registration API
}
````

> You should install the [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) nuget package to the project that you want to use the Autofac API.
