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