# Dealing with Multiple Implementations of a Service in ASP.NET Core & ABP Dependency Injection

ASP.NET Core provides a built-in [dependency injection system](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) to register your services to the dependency injection container and inject/resolve them whenever you need. ABP's [dependency injection infrastructure](https://docs.abp.io/en/abp/latest/Dependency-Injection) is built on ASP.NET Core's DI system, automates service registration by conventions and provides some additional features.

In this tutorial, I will explain how you can register multiple implementations of the same service interface and inject/resolve all these implementations when you need them.

## Defining Services

Assume that you have an `IExternalLogger` interface with two implementations:

````csharp
public interface IExternalLogger
{
    Task LogAsync(string logText);
}

public class ElasticsearchExternalLogger : IExternalLogger
{
    public async Task LogAsync(string logText)
    {
        // TODO...
    }
}

public class AzureExternalLogger : IExternalLogger
{
    public async Task LogAsync(string logText)
    {
        // TODO...
    }
}
````

An example service injecting the `IExternalLogger` interface:

````csharp
public class MyService : ITransientDependency
{
    private readonly IExternalLogger _externalLogger;

    public MyService(IExternalLogger externalLogger)
    {
        _externalLogger = externalLogger;
    }

    public async Task DemoAsync()
    {
        await _externalLogger.LogAsync("Example log message...");
    }
}
````

In this example, we haven't registered any of the implementation classes to the dependency injection system yet. So, if we try to use the `MyService` class, we get an error indicating that no implementation found for the `IExternalLogger` service. We should register at least one implementation for the `IExternalLogger` interface.

## Registering Services

If we register both of the `ElasticsearchExternalLogger` and `AzureExternalLogger` services for the `IExternalLogger` interface, and then try to inject the `IExternalLogger` interface, the last registered implementation will be used. However, how to determine the last registered implementation?

If we implement one of the [dependency interfaces](https://docs.abp.io/en/abp/latest/Dependency-Injection#dependency-interfaces) (e.g. `ITransientDependency`), then the registration order will be uncertain (it may depend on the namespaces of the classes). The *last registered implementation* can be different than you expect. So, it is not suggested to use the dependency interfaces to register multiple implementations.

You can register your services in the `ConfigureServices` method of your module:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<IExternalLogger, ElasticsearchExternalLogger>();
    context.Services.AddTransient<IExternalLogger, AzureExternalLogger>();
}
````

In this case, you get an `AzureExternalLogger` instance when you inject the `IExternalLogger` interface, because the last registered implementation is the `AzureExternalLogger` class.

## Injecting Multiple Implementations

When you have multiple implementation of an interface, you may want to work with all these implementations. For this example, you may want to write log to all the external loggers. We can change the `MyService` implementation as the following:

````csharp
public class MyService : ITransientDependency
{
    private readonly IEnumerable<IExternalLogger> _externalLoggers;

    public MyService(IEnumerable<IExternalLogger> externalLoggers)
    {
        _externalLoggers = externalLoggers;
    }

    public async Task DemoAsync()
    {
        foreach (var externalLogger in _externalLoggers)
        {
            await externalLogger.LogAsync("Example log message...");
        }
    }
}
````

In this example, we are injecting `IEnumerable<IExternalLogger>` instead of `IExternalLogger`, so we have a collection of the `IExternalLogger` implementations. Then we are using a `foreach` loop to write the same log text to all the `IExternalLogger` implementations.

If you are using `IServiceProvider` to resolve dependencies, then use its `GetServices` method to obtain a collection of the service implementations:

````csharp
IEnumerable<IExternalLogger> services = _serviceProvider.GetServices<IExternalLogger>();
````

## Further Reading

In this small tutorial, I explained how you can register multiple implementations of the same interface to the dependency injection system and inject/resolve all of them when you need.

If you want to get more information about ABP's and ASP.NET Core's dependency injection systems, you can read the following documents:

* [ABP's Dependency Injection documentation](https://docs.abp.io/en/abp/latest/Dependency-Injection)
* [ASP.NET Core Dependency Injection best practices, tips & tricks](https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96)
* [ASP.NET Core's Dependency Injection documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)