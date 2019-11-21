﻿# Getting Started ABP With AspNet Core MVC Web Application

This tutorial explains how to start ABP from scratch with minimal dependencies. You generally want to start with the **[startup template](Getting-Started-AspNetCore-MVC-Template.md)**.

## Create A New Project

1. Create a new AspNet Core Web Application from Visual Studio 2019 (16.3.0+):

![](images/create-new-aspnet-core-application-v2.png)

2. Configure your new project:

![](images/select-empty-web-application-v2.png)

3. Press to the create button:

![create-aspnet-core-application](images/create-aspnet-core-application.png)

## Install Volo.Abp.AspNetCore.Mvc Package

Volo.Abp.AspNetCore.Mvc is AspNet Core MVC integration package for ABP. So, install it to your project:

````
Install-Package Volo.Abp.AspNetCore.Mvc
````

## Create First ABP Module

ABP is a modular framework and it requires a **startup (root) module** class derived from ``AbpModule``:

````C#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace BasicAspNetCoreApplication
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseMvcWithDefaultRouteAndArea();
        }
    }
}
````

``AppModule`` is a good name for the startup module for an application.

ABP packages define module classes and a module can depend on another. In the code above, the ``AppModule`` depends on the ``AbpAspNetCoreMvcModule`` (defined by [Volo.Abp.AspNetCore.Mvc](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc) package). It's common to add a ``DependsOn`` attribute after installing a new ABP nuget package.

Instead of the Startup class, we are configuring ASP.NET Core pipeline in this module class.

## The Startup Class

Next step is to modify Startup class to integrate to ABP module system:

````C#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BasicAspNetCoreApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AppModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
````

``services.AddApplication<AppModule>()`` adds all services defined in all modules starting from the ``AppModule``.

``app.InitializeApplication()`` in ``Configure`` method initializes and starts the application.

## Run the Application!

That's all! Run the application, it will just work as expected.

## Using Autofac as the Dependency Injection Framework

While AspNet Core's Dependency Injection (DI) system is fine for basic requirements, [Autofac](https://autofac.org/) provides advanced features like Property Injection and Method Interception which are required by ABP to perform advanced application framework features.

Replacing AspNet Core's DI system by Autofac and integrating to ABP is pretty easy.

1. Install [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) package

````
Install-Package Volo.Abp.Autofac
````

2. Add ``AbpAutofacModule`` Dependency

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))] //Add dependency to ABP Autofac module
public class AppModule : AbpModule
{
    ...
}
````

3. Update `Program.cs` to use Autofac:

````csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BasicAspNetCoreApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac(); //Add this line
    }
}
````

## Source Code

Get source code of the sample project created in this tutorial from [here](https://github.com/abpframework/abp/tree/master/samples/BasicAspNetCoreApplication).

