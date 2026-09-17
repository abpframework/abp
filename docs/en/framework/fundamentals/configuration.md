```json
//[doc-seo]
{
    "Description": "Explore how to leverage ASP.NET Core's flexible configuration system with ABP Framework for effective application management and customization."
}
```

# Configuration

ASP.NET Core has an flexible and extensible key-value based configuration system. The configuration system is a part of Microsoft.Extensions libraries and it is independent from ASP.NET Core. That means it can be used in any type of application.

See [Microsoft's documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) to learn the configuration infrastructure. ABP is 100% compatible with the configuration system.

## Getting the Configuration

You may need to get the `IConfiguration` service in various places in your codebase. The following section shows two common ways.

### In Module Classes

You typically need to get configuration while initializing your application. You can get the `IConfiguration` service using the `ServiceConfigurationContext.Configuration` property inside your [module class](../architecture/modularity/basics.md) as the following example:

````csharp
public class MyAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var connectionString = context.Configuration["ConnectionStrings:Default"];
    }
}
````

`context.Configuration` is a shortcut property for the `context.Services.GetConfiguration()` method. In general, prefer using `context.Configuration` for simplicity and readability when working within module classes. Use `context.Services.GetConfiguration()` in other contexts where you have an `IServiceCollection` object but do not have access to the `context.Configuration` property. (`IServiceCollection.GetConfiguration` is an extension method that can be used whenever you have an `IServiceCollection` object).

### In Your Services

You can directly [inject](dependency-injection.md) the `IConfiguration` service into your services:

````csharp
public class MyService : ITransientDependency
{
    private readonly IConfiguration _configuration;

    public MyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string? GetConnectionString()
    {
        return _configuration["ConnectionStrings:Default"];
    }
}
````

## See Also

* [Microsoft's Configuration Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
* [The Options Pattern](options.md)

