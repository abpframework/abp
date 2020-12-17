# Plug-In Modules

It is possible to load [modules](Module-Development-Basics.md) as plug-ins. That means you may not reference to a module's assembly in your solution, but you can load that module in the application startup just like any other module.

## Basic Usage

`IServiceCollection.AddApplication<T>()` extension method can get options to configure the plug-in sources.

**Example: Load plugins from a folder**

````csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace MyPlugInDemo.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<MyPlugInDemoWebModule>(options =>
            {
                options.PlugInSources.AddFolder(@"D:\Temp\MyPlugIns");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
````

* This is the `Startup` class of a typical ASP.NET Core application.
* `PlugInSources.AddFolder` gets a folder path and to load assemblies (typically `dll`s) in that folder.

That's all. ABP will discover the modules in the given folder, configure and initialize them just like regular modules.

### Plug-In Sources

`options.PlugInSources` is actually a list of `IPlugInSource` implementations and `AddFolder` is just a shortcut for the following expression:

````csharp
options.PlugInSources.Add(new FolderPlugInSource(@"D:\Temp\MyPlugIns"));
````

> `AddFolder()` only looks for the assembly file in the given folder, but not looks for the sub-folders. You can pass `SearchOption.AllDirectories` as a second parameter to explore plug-ins also from the sub-folders, recursively.

There are two more built-in Plug-In Source implementations:

* `PlugInSources.AddFiles()` gets a list of assembly (typically `dll`) files. This is a shortcut of using `FilePlugInSource` class.
* `PlugInSources.AddTypes()` gets a list of module class types. If you use this, you need to load the assemblies of the modules yourself, but it provides flexibility when needed. This is a shortcut of using `TypePlugInSource` class.

If you need, you can create your own `IPlugInSource` implementation and add to the `options.PlugInSources` just like the others.

## Creating a Simple Plug-In

Create a simple **Class Library Project** in a solution:

![simple-plugin-library](images/simple-plugin-library.png)

You can add ABP Framework packages you need to use in the module. At least, you should add the `Volo.Abp.Core` package to the project:

````
Install-Package Volo.Abp.Core
````

Every [module](Module-Development-Basics.md) must declare a class derived from the `AbpModule`. Here, a simple module class that resolves a service and initializes it on the application startup:

````csharp
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MyPlugIn
{
    public class MyPlungInModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var myService = context.ServiceProvider
                .GetRequiredService<MyService>();
            
            myService.Initialize();
        }
    }
}
````

`MyService` can be any class registered to [Dependency Injection](Dependency-Injection.md) system, as show below:

````csharp
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace MyPlugIn
{
    public class MyService : ITransientDependency
    {
        private readonly ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("MyService has been initialized");
        }
    }
}
````

Build the project, open the build folder, find the `MyPlugIn.dll`:

![simple-plug-in-dll-file](images/simple-plug-in-dll-file.png)

Copy `MyPlugIn.dll` into the plug-in folder (`D:\Temp\MyPlugIns` for this example).

If you have configured the main application like described above (see Basic Usage section), you should see the `MyService has been initialized` log in the application startup.