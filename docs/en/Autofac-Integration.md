# Autofac Integration

Autofac is one of the most used dependency injection frameworks for .Net. It provides some advanced features compared to .Net Core standard DI library, like dynamic proxying and property injection.

## Install Autofac Integration

> All startup templates and samples are Autofac integrated. So, most of the time you don't need to manually install this package.

Install [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) nuget package to your project (for a multi-projects application, it's suggested to add to the executable/web project.)

````
Install-Package Volo.Abp.Autofac
````

Then add `AbpAutofacModule` dependency to your module:

```csharp
using Volo.Abp.Modularity;
using Volo.Abp.Autofac;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

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

