## Module Development

### Introduction

ABP itself is a modular framework. It also provides infrastructure and architectural model to develop your own modules.

### Module Class

Every module should define a module class. Most simple way of defining a module class is to create a class derived from ``AbpModule`` as like below:

````C#
public class BlogModule : AbpModule
{
            
}

````

#### Configuring Dependency Injection & Other Modules

##### ConfigureServices Method

``ConfigureServices`` is the main method to add your services to dependency injection system and configure other modules. Example:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        //...
    }
}
````

You can register dependencies one by one as stated in Microsoft's documentation (TODO: link). ABP has also a **conventional dependency registration system** which allows you to register all services in your assembly automatically. ``ConfigureServices`` methods of most modules contain such an expression to register all services in given module:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAssemblyOf<BlogModule>();
    }
}
````

See Dependency Injection (TODO: link) documentation for more about the dependency injection system.

You can also configure other services and modules in this method. Example:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAssemblyOf<BlogModule>();

        //Configure default connection string for the application
        services.Configure<DbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = "......";
        });
    }
}
````

See Configuration (TODO: link) document for more about the configuration system.

##### Pre & Post Configure Services

``AbpModule`` class also defines ``PreConfigureServices`` and ``PostConfigureServices`` methods to override and write your code just before and just after ``ConfigureServices``. Notice that the code you have written into these methods will be executed before/after ``ConfigureServices`` methods of all other modules.

#### Application Initialization

Once all services of all modules are configured, application starts by initializing all modules. In this phase, you can resolve services from ``IServiceProvider`` since it's ready and available.

##### OnApplicationInitialization Method

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

``OnApplicationInitialization`` is generally used by the startup module to construct middleware pipeline for ASP.NET Core applications. Example:

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

You can also perform startup logic if your module requires

##### Pre & Post Application Initialization

``AbpModule`` class also defines ``OnPreApplicationInitialization`` and ``OnPostApplicationInitialization`` methods to override and write your code just before and just after ``OnApplicationInitialization``. Notice that the code you have written into these methods will be executed before/after ``OnApplicationInitialization`` methods of all other modules.

#### Application Shutdown

Lastly, you can override ``OnApplicationShutdown`` method if you want to execute a code while application is beign shutdown.

#### Alternative to Deriving from AbpModule Class

If you want to not derive your modules from ``AbpModule`` class for some reason, you can create a class and implement ``IAbpModule`` interface. This is the minimal interface required by ABP. If you want to handle other life cycle events as described above, you can implement additional interfaces:

* ``IPreConfigureServices``
* ``IPostConfigureServices``
* ``IOnPreApplicationInitialization``
* ``IOnApplicationInitialization``
* ``IOnPostApplicationInitialization``
* ``IOnApplicationShutdown``

However, deriving from ``AbpModule`` class is simpler since it implements all of these interfaces as virtual empty methods, so you can simple override which you need.

### Module Dependencies

In a modular application, it's typical a module depends on other modules. An Abp module must declare ``[DependsOn]`` attribute if it depends on another module, as shown below:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))]
public class BlogModule
{
    //...
}
````

You can use multiple ``DependsOn`` attribute or pass multiple module types to a single ``DependsOn`` attribute depending on your preference.

A depended module may depend on another module, but you only need to define your direct dependencies. ABP investigates dependency graph on application startup and initializes/shutdowns modules in the correct order.