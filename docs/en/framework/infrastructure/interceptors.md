# Interceptors

ABP provides a powerful interception system that allows you to execute custom logic before and after method calls without modifying the original method code. This is achieved through **dynamic proxying** and is extensively used throughout the ABP framework to implement cross-cutting concerns. ABP's interception is implemented on top of the [Castle DynamicProxy](https://www.castleproject.org/projects/dynamicproxy/) library.

## What is Dynamic Proxying / Interception?

**Interception** is a technique that allows executing additional logic before or after a method call without directly modifying the method's code. This is achieved through **dynamic proxying**, where the runtime generates proxy classes that wrap the original class.

When a method is called on a proxied object:
1. The call is intercepted by the proxy
2. Custom behaviors (like logging, validation, or authorization) can be executed
3. The original method is called
4. Additional logic can be executed after the method completes

This enables **cross-cutting concerns** (logic that applies across many parts of an application) to be handled in a clean, reusable way without code duplication.

## Similarities and Differences with MVC Action/Page Filters

If you are familiar with ASP.NET Core MVC, you've likely used **action filters** or **page filters**. Interceptors are conceptually similar but have some key differences:

### Similarities

* Both allow executing code before and after method execution
* Both are used to implement cross-cutting concerns like validation, logging, caching, or exception handling
* Both support asynchronous operations

### Differences

* **Scope**: Filters are tied to MVC's request pipeline, while interceptors can be applied to **any class or service** in the application
* **Configuration**: Filters are configured via attributes or middleware in MVC, while interceptors in ABP are applied through **dependency injection and dynamic proxies**
* **Target**: Interceptors can target application services, domain services, repositories, and virtually any service resolved from the IoC container—not just web controllers

## How ABP Uses Interceptors

ABP Framework extensively leverages interception to provide built-in features without requiring boilerplate code. Here are some key examples:

### [Unit of Work (UOW)](../architecture/domain-driven-design/unit-of-work.md)

Automatically begins and commits/rolls back a database transaction when entering or exiting an application service method. This ensures data consistency without manual transaction management.

### [Input Validation](../fundamentals/validation.md)

Input DTOs are automatically validated against data annotation attributes and custom validation rules before executing the service logic, providing consistent validation behavior across all services.

### [Authorization](../fundamentals/authorization/index.md)

Checks user permissions before allowing the execution of application service methods, ensuring security policies are enforced consistently.

### [Feature](./features.md) & [Global Feature](./global-features.md) Checking

Checks if a feature is enabled before executing the service logic, allowing you to conditionally enable or restrict functionality for tenants or the application.

### [Auditing](./audit-logging.md)

Automatically logs who performed an action, when it happened, what parameters were used, and what data was involved, providing comprehensive audit trails.

## Building Your Own Interceptor

You can create custom interceptors in ABP to implement your own cross-cutting concerns.

### Creating an Interceptor

Create a class that inherits from `AbpInterceptor`:

````csharp
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

public class ExecutionTimeLogInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly ILogger<ExecutionTimeLogInterceptor> _logger;

    public ExecutionTimeLogInterceptor(ILogger<ExecutionTimeLogInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Executing {invocation.TargetObject.GetType().Name}.{invocation.Method.Name}");

        // Proceed to the actual target method
        await invocation.ProceedAsync();

        sw.Stop();

        _logger.LogInformation($"Executed {invocation.TargetObject.GetType().Name}.{invocation.Method.Name} in {sw.ElapsedMilliseconds} ms");
    }
}
````

### Register Interceptors

Create a static class that contains the `RegisterIfNeeded` method and register the interceptor in the `PreConfigureServices` method of your module.

The `ShouldIntercept` method is used to determine if the interceptor should be registered for the given type. You can add an `IExecutionTimeLogEnabled` interface and implement it in the classes that you want to intercept.

> `DynamicProxyIgnoreTypes` is static class that contains the types that should be ignored by the interceptor. See [Performance Considerations](#performance-considerations) for more information.

````csharp
// Define an interface to mark the classes that should be intercepted
public interface IExecutionTimeLogEnabled
{
}
````

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

// A simple service that added to the DI container and will be intercepted since it implements the `IExecutionTimeLogEnabled` interface
public class SampleExecutionTimeService : IExecutionTimeLogEnabled, ITransientDependency
{
    public virtual async Task DoWorkAsync()
    {
        // Simulate a long-running task to test the interceptor
        await Task.Delay(1000);
    }
}
````

````csharp
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

public static class ExecutionTimeLogInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<ExecutionTimeLogInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        return !DynamicProxyIgnoreTypes.Contains(type) && typeof(IExecutionTimeLogEnabled).IsAssignableFrom(type);
    }
}
````

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    context.Services.OnRegistered(ExecutionTimeLogInterceptorRegistrar.RegisterIfNeeded);
}
````

## Advanced Topics

### Intercepting Methods Returning ValueTask

A method returning `ValueTask` or `ValueTask<T>` is intercepted like a method returning `Task` or `Task<T>`: the interceptor runs until the returned `ValueTask` completes, and `invocation.ReturnValue` contains the result of the method. A method with a `ref`, `out` or `in` parameter is intercepted synchronously, because the proxy has to set these parameters when the method returns.

### Intercepting Methods Returning IAsyncEnumerable

A method returning `IAsyncEnumerable<T>` is intercepted like a method returning `Task<T>`: the interceptor runs when the method is called, and `invocation.ProceedAsync()` only creates the sequence. The sequence does its work later, when the caller enumerates it, so an interceptor does not cover the enumeration by default. If an interceptor throws (for example, an authorization or validation error), the exception is thrown when the sequence is enumerated.

To also cover the enumeration, inherit your interceptor from `AbpScopeInterceptor` and return an `IAbpInterceptionScope` from the `BeginScope` method. The scope runs around the method call, and a new scope runs around each enumeration of the returned sequence. For example, you can change the `ExecutionTimeLogInterceptor` above as below to also measure the enumerations:

````csharp
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

public class ExecutionTimeLogInterceptor : AbpScopeInterceptor, ITransientDependency
{
    private readonly ILogger<ExecutionTimeLogInterceptor> _logger;

    public ExecutionTimeLogInterceptor(ILogger<ExecutionTimeLogInterceptor> logger)
    {
        _logger = logger;
    }

    protected override IAbpInterceptionScope BeginScope(IAbpMethodInvocation invocation)
    {
        return new ExecutionTimeLogScope(_logger, invocation.Method.Name);
    }
}

public class ExecutionTimeLogScope : IAbpInterceptionScope
{
    private readonly ILogger _logger;
    private readonly string _methodName;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public ExecutionTimeLogScope(ILogger logger, string methodName)
    {
        _logger = logger;
        _methodName = methodName;
    }

    public Task CompleteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Executed {_methodName} in {_stopwatch.ElapsedMilliseconds} ms");
        return Task.CompletedTask;
    }

    public Task FailAsync(Exception exception)
    {
        _logger.LogWarning($"{_methodName} failed after {_stopwatch.ElapsedMilliseconds} ms");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
````

* `CompleteAsync` is called when the method call or the enumeration completes, including when the caller stops enumerating early (for example, with `break`).
* `FailAsync` is called when the method or the sequence throws an exception.
* `Dispose` is always called at the end.

`AbpInterceptionScope` is a default implementation that disposes a given object, which is useful to change an ambient value during the scope. For example, `new AbpInterceptionScope(_currentTenant.Change(tenantId))` switches the current tenant during the method call and each enumeration.

If your interceptor can not inherit from `AbpScopeInterceptor`, call the `ProceedInScopeAsync` extension method in the `InterceptAsync` method instead of `invocation.ProceedAsync()`:

````csharp
public override async Task InterceptAsync(IAbpMethodInvocation invocation)
{
    await invocation.ProceedInScopeAsync(() => BeginScope(invocation));
}
````

ABP's [Unit of Work](../architecture/domain-driven-design/unit-of-work.md), [Auditing](./audit-logging.md) and entity change tracking interceptors work this way, while the authorization, validation and feature checks are only performed when the method is called.

> `BeginScope` must set its async local values synchronously (for example, it should not be an `async` method). Otherwise, the values are not available to the method and to the enumeration.

> Since the authorization and other checks are performed when the method is called, you should enumerate the returned sequence in the same context (the same user and tenant) as the method call.

> A method with a `ref`, `out` or `in` parameter is intercepted only when it is called, so its enumerations are not covered by a scope.

## Restrictions and Important Notes

### Always use asynchronous methods

For best performance and reliability, implement your service methods as asynchronous to avoid **async over sync**, that can cause unexpected problems, For more information, see [Should I expose synchronous wrappers for asynchronous methods?](https://devblogs.microsoft.com/dotnet/should-i-expose-synchronous-wrappers-for-asynchronous-methods/)

### Virtual Methods Requirement

For **class proxies**, methods need to be marked as `virtual` so that they can be overridden by the proxy. Otherwise, interception will not occur.

````csharp
public class MyService : IExecutionTimeLogEnabled, ITransientDependency
{
    // This method CANNOT be intercepted (not virtual)
    public void CannotBeIntercepted()
    {
    }

    // This method CAN be intercepted (virtual)
    public virtual void CanBeIntercepted()
    {
    }
}
````

> This restriction does **not** apply to interface-based proxies. If your service implements an interface and is injected via that interface, all methods can be intercepted regardless of the `virtual` keyword.

### Dependency Injection Scope

Interceptors only work when services are resolved from the dependency injection container. Direct instantiation with `new` bypasses interception:

````csharp
// This will NOT be intercepted
var service = new MyService();
service.CannotBeIntercepted();

// This WILL be intercepted (if MyService is registered with DI)
var service = serviceProvider.GetService<MyService>();
service.CanBeIntercepted();
````

### Performance Considerations

Interceptors are generally efficient, but each one adds method-call overhead. Keep the number of interceptors minimal on hot paths.

Castle DynamicProxy can negatively impact performance for certain components, notably ASP.NET Core MVC controllers. See the discussions in [castleproject/Core#486](https://github.com/castleproject/Core/issues/486) and [abpframework/abp#3180](https://github.com/abpframework/abp/issues/3180).

ABP uses interceptors for features like UOW, auditing, and authorization, which rely on dynamic proxy classes. For controllers, prefer implementing cross-cutting concerns with middleware or MVC/Page filters instead of dynamic proxies.

To avoid generating dynamic proxies for specific types, use the static class `DynamicProxyIgnoreTypes` and add the base classes of the types to the list. Subclasses of any listed base class are also ignored. ABP framework already adds some base classes to the list (`ComponentBase, ControllerBase, PageModel, ViewComponent`); you can add more base classes if needed.

You can also disable ABP class interceptors for all registrations or for types selected by a predicate:

````csharp
// Disable all class interceptors.
context.Services.DisableAbpClassInterceptors();

// Or disable them only for selected types. The predicate runs for class
// service registrations and receives the exposed class service type,
// which is the exposed base class rather than the implementation type
// when a class is exposed through a base class.
context.Services.DisableAbpClassInterceptors(
    new NamedTypeSelector(
        "MyHotPathServices",
        type => type.Namespace == "MyProject.HotPath"
    )
);
````

These methods control class interception. Interface-based interception is configured separately.

> Always use interface-based proxies instead of class-based proxies for better performance.

## See Also

* [Video tutorial: Interceptors in ABP Framework](https://abp.io/video-courses/essentials/interception)
* [Castle DynamicProxy](https://www.castleproject.org/projects/dynamicproxy/)
* [Castle.Core.AsyncInterceptor](https://github.com/JSkimming/Castle.Core.AsyncInterceptor)
* [ASP.NET Core Filters](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters)
