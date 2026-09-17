# Middleware Now Supports Keyed Dependency Injection in .NET 9

This article explores a new feature in .NET 9 that enables keyed dependency injection in middleware. Previously, .NET 8 introduced keyed services, which allowed developers to register multiple instances of the same service type with distinct keys. Now, .NET 9 extends this feature to middleware, making it easier to inject specific services within the middleware based on defined keys. For more details, see this [overview on the .NET blog](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#keyed-di-in-middleware).

## What is Keyed Dependency Injection?

Keyed dependency injection is a technique for registering multiple service versions with unique identifiers, or “keys.” This approach is especially helpful when multiple implementations of the same service are required in different contexts. For example, you may have various logging services but want to inject a specific logger based on the application’s current needs. By using keys, developers can ensure that the appropriate service version is injected precisely where it’s needed.

## Using Keyed Dependency Injection in Middleware

In .NET 9, developers can now use keyed dependency injection directly in middleware. Keyed services can be injected through the middleware constructor or via the `Invoke`/`InvokeAsync` methods, allowing for straightforward and flexible control of service instances in middleware components. Here’s an example of how to configure and use keyed dependency injection in middleware:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register services with unique keys
builder.Services.AddKeyedSingleton<MySingletonClass>("test");
builder.Services.AddKeyedScoped<MyScopedClass>("test2");

var app = builder.Build();
app.UseMiddleware<MyMiddleware>();
app.Run();

internal class MyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MySingletonClass _singletonService;

    // Constructor injection with key
    public MyMiddleware(RequestDelegate next, [FromKeyedServices("test")] MySingletonClass singletonService)
    {
        _next = next;
        _singletonService = singletonService;
    }

    // Invoke method with additional scoped service injection using key
    public Task Invoke(HttpContext context, [FromKeyedServices("test2")] MyScopedClass scopedService)
    {
        // Middleware logic here
        return _next(context);
    }
}
```

In this example:
- `MySingletonClass` and `MyScopedClass` are registered with unique keys (`"test"` and `"test2"`).
- These services are injected into the middleware through both the constructor and `Invoke` method, based on their respective keys.

This approach allows developers to manage which service instances are available within middleware precisely.

## Conclusion

Keyed dependency injection in middleware is a significant addition in .NET 9. It provides developers with more control over which services are injected based on specific keys. This enhancement enables selective service injection in middleware scenarios, allowing for more modular and maintainable applications.

## References

- [.NET 9 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/rc1/aspnetcore.md#keyed-di-in-middleware)
- [Dependency Injection and Keyed Services](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection#keyed-services)