## Getting Started ABP With Console Application

### Create A New Project

Create a new Regular .Net Core Console Application from Visual Studio:

![](images/create-new-net-core-console-application.png)

### Install Volo.Abp Package

Volo.Abp is the core nuget package to create ABP based applications. So, install it to your project:

````
Install-Package Volo.Abp
````

### Create First ABP Module

ABP is a modular framework and it requires a **startup (root) module** class derived from ``AbpModule``:

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpConsoleDemo
{
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AppModule>();
        }
    }
}
````

``AppModule`` is a good name for the startup module for a console application. A module class can register services to Dependency Injection by overriding ``ConfigureServices`` method as shown here. ``AddAssemblyOf<...>`` is a special extension method of ABP that registers all services in an assembly by convention (TODO: link to DI document). While this is optional, a module generally registers some services.

### Initialize The Application

The next step is to bootstrap the application using the startup module created above:

````C#
using System;
using Volo.Abp;

namespace AbpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AppModule>())
            {
                application.Initialize();

                Console.WriteLine("Press ENTER to stop application...");
                Console.ReadLine();
            }
        }
    }
}

````

``AbpApplicationFactory`` is used to create the application and load all modules taking ``AppModule`` as the startup module. ``Initialize()`` method starts the application.

### Hello World!

The application above does nothing. Let's create a service does something:

````C#
using System;
using Volo.Abp.DependencyInjection;

namespace AbpConsoleDemo
{
    public class HelloWorldService : ITransientDependency
    {
        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}

````

``ITransientDependency`` is a special interface of ABP that automatically registers the service as transient (TODO: link to MS DI documentation and ABP DI documentation).

Now, we can resolve the ``HelloWorldService`` and say hello. Change the Program.cs as shown below:

````C#
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace AbpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AppModule>())
            {
                application.Initialize();

                //Resolve a service and use it
                var helloWorldService = application.ServiceProvider.GetService<HelloWorldService>();
                helloWorldService.SayHello();

                Console.WriteLine("Press ENTER to stop application...");
                Console.ReadLine();
            }
        }
    }
}
````

While it's enough for this simple code example, it's always suggested to create scopes in case of directly resolving dependencies from ``IServiceProvider`` (TODO: see DI documentation).