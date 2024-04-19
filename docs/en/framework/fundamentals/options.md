# Options

Microsoft has introduced [the options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) that is used to configure a group of settings used by the framework services. This pattern is implemented by the [Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options) NuGet package, so it is usable by any type of applications in addition to ASP.NET Core based applications.

ABP framework follows this option pattern and defines options classes to configure the framework and the modules (they are explained in the documents of the related feature).

Since [the Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) explains the pattern in detail, no reason to repeat all. However, ABP adds a few more features and they will be explained here.

## Configure Options

You typically configure options in the `ConfigureServices` of the `Startup` class. However, since ABP framework provides a modular infrastructure, you configure options in the `ConfigureServices` of your [module](Module-Development-Basics.md). Example:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpAuditingOptions>(options =>
    {
        options.IsEnabled = false;
    });
}
````

* `AbpAuditingOptions` is a simple class defines some properties like `IsEnabled` used here.
* `AbpModule` base class defines `Configure` method to make the code simpler. So, instead of `context.Services.Configure<...>`, you can directly use the `Configure<...>` shortcut method.

If you are developing a reusable module, you may need to define an options class to allow developers to configure your module. In this case, define a plain options class as shown below:

````csharp
public class MyOptions
{
    public int Value1 { get; set; }
    public bool Value2 { get; set; }
}
````

Then developers can configure your options just like the `AbpAuditingOptions` example above:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<MyOptions>(options =>
    {
        options.Value1 = 42;
        options.Value2 = true;
    });
}
````

* In this example, used the shortcut `Configure<...>` method.

### Get the Option Value

Whenever you need to get the value of an option, [inject](Dependency-Injection.md) the `IOptions<TOption>` service into your class and use its `.Value` property. Example:

````csharp
public class MyService : ITransientDependency
{
    private readonly MyOptions _options;
    
    public MyService(IOptions<MyOptions> options)
    {
        _options = options.Value; //Notice the options.Value usage!
    }

    public void DoIt()
    {
        var v1 = _options.Value1;
        var v2 = _options.Value2;
    }
}
````

Read [the Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) for all details of the options pattern.

## Pre Configure

One restriction of the options pattern is that you can only resolve (inject) the `IOptions<MyOptions>` and get the option values when the dependency injection configuration completes (that means the `ConfigureServices` methods of all modules complete).

If you are developing a module, you may need to allow developers to set some options and use these options in the dependency injection registration phase. You may need to configure other services or change the dependency injection registration code based on these option values.

For such cases, ABP introduces the `PreConfigure<TOptions>` and the `ExecutePreConfiguredActions<TOptions>` extension methods for the `IServiceCollection`. The pattern works as explained below.

Define a pre option class in your module. Example:

````csharp
public class MyPreOptions
{
    public bool MyValue { get; set; }
}
````

Then any [module class](Module-Development-Basics.md) depends on your module can use the `PreConfigure<TOptions>` method in its `PreConfigureServices` method. Example:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<MyPreOptions>(options =>
    {
        options.MyValue = true;
    });
}
````

> Multiple modules can pre-configure the options and override the option values based on their dependency order.

Finally, your module can execute the `ExecutePreConfiguredActions` method in its `ConfigureServices` method to get the configured option values. Example:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var options = context.Services.ExecutePreConfiguredActions<MyPreOptions>();
    if (options.MyValue)
    {
        //...
    }
}
````

