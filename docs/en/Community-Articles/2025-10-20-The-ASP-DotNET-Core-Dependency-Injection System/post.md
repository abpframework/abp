# The ASP.NET Core Dependency Injection System

## Article Overview

This article provides a guide to **ASP.NET Core Dependency Injection**, a fundamental element of .NET development. We'll examine the built in Inversion of Control (IoC) container, examining the critical differences between service lifecycles (scoped, singleton, or transient), and comparing constructor injection to property injection.

You'll learn how to effectively register your services with patterns like `TryAdd` methods and the **Options Pattern**, how to adhere to established best practices like avoiding captive dependencies and asynchronous constructor logic, how to understand patterns like decorators and explicit generics, how to leverage manual scope management with `IServiceScopeFactory`, how to implement proper asynchronous disposal with `IAsyncDisposable`, and how to analyze the performance impact of your DI strategy, including the new compile time source generation features that enable native AOT support in .NET 9. Ultimately, you'll have the knowledge to create loosely coupled, maintainable, and testable applications using the advanced dependency injection patterns in .NET 9. Of course, the explanations here are general, if you'd like to delve deeper, you can check out the references section.

## Introduction to Dependency Injection

Dependency Injection (DI) is a design pattern used to implement Inversion of Control (IoC), in which control of object creation and binding is transferred from the object itself to another container or framework. In the context of ASP.NET Core, DI is a tool integrated into the framework for managing the lifecycle and creation of application components.

### The Evolution from .NET Framework to .NET Core and .NET 9

The journey of dependency injection in the .NET ecosystem represents a sweeping architectural shift. .NET Framework applications (prior to 2016) typically relied on third party IoC containers such as

- **Unity** (Microsoft's own container, often used in enterprise applications)
- **Autofac** (popular for its advanced features and fluent API)
- **Ninject** (known for its simplicity)
- **StructureMap** (one of the earliest .NET IoC containers)
- **Castle Windsor** (powerful but complex)

Many legacy applications have fallen back to anti patterns like the **Service Locator pattern**, which hides dependencies and makes testing difficult.

When ASP.NET Core was released in 2016, Microsoft made a decision to integrate dependency injection directly into the runtime. This meant:

- **Standardization:** Creating a consistent DI approach across all .NET Core applications.
- **Performance:** Creating a lightweight, optimized container designed for workloads.
- **Simplicity:** No need to choose third party containers for basic scenarios.
- **Cloud Native Ready:** Designed for microservices, containers, and serverless architectures.

Now, with **.NET 9**, the DI container has become even more advanced as follows:

- **Source generated DI** for faster startup and AOT compatibility.
- **Keyed services** for advanced solution scenarios.
- **Lifetime validation** to catch common errors during development.

Unlike .NET Framework applications, which required installing these containers as third-party packages and often present issues with consistency, in ASP.NET Core, and now in .NET 9, dependency injection is a fundamental part of the architecture.

### Why is Dependency Injection important?

The key benefits of adopting a DI strategy would be.

  * **Loose Coupling:** Components don't create their dependencies directly. Instead, they get them from the DI container. This means you can change the implementation of a dependency without changing the component that uses it.
  * **Testability:** Once dependencies are added, you can easily replace them with mocks or mock implementations in your unit tests. This will allow you to test components in isolation.
  * **Maintenance and Scalability:** A loosely coupled architecture is easier to manage, refactor, and extend. New features can be added with minimal changes to existing code.

This article provides a guide for developers covering the basic mechanisms of Dependency Injection in .NET and the performance models available in .NET 9.

## The Built in IoC Container in ASP.NET Core

ASP.NET Core ships with the lightweight yet comprehensive **ASP.NET Core IoC container**. It's not designed to have all the features of third party tools, but it provides the basic functionality needed for most applications.

The two basic interfaces representing the structure are as follows:

  * `IServiceCollection`: This is the "registration" side of the structure. When the application starts up, you add your services or dependencies to this collection.
  * `IServiceProvider`: This is the "resolving" side of the structure. After the application is created, `IServiceProvider` is used to retrieve instances of registered services.

### Comparison with Third Party Tools

While the internal structure is sufficient for many scenarios, you can easily modify it if you need more features, such as:

  * **Automatic registration / Assembly Scan:** Automatically register types based on contracts.
  * **Interception / Decorators:** Providing more support for packaging services with cross cutting concerns.
  * **Child Containers:** Some tools, such as Autofac, allow you to create nested sub containers with their own lifetimes, which can be useful for isolating components in complex applications. The built in framework uses a simpler scoping mechanism.


## Service Lifetimes in ASP.NET Core

When registering a service, you must specify its lifetime. The lifetime determines how long a service instance will be valid. Understanding the difference between **scoped, singleton, and transient** is important for building stable and optimized applications.

### Transient

A new instance of a transient service is created **each time** it is requested from the container.

  * **When to use:** For lightweight, stateless services.
  * `builder.Services.AddTransient<IMessageSender, EmailSender>();`

### Scoped

A single instance of a scoped service is created once per client request (or per scope). The same instance is shared within that single request.

  * **When to use:** It is more appropriate to use it for services that need to maintain state within a single request, such as `DbContext` or Unit of Work.
  * `builder.Services.AddScoped<IUserRepository, UserRepository>();`

### Singleton

A single instance of the service is created once during the entire application lifetime.

  * **When to use:** Commonly used for stateless services that are source intensive to create or need to share their state extensively, such as application configuration or caching services.
  * `builder.Services.AddSingleton<IAppCache, MemoryCacheService>();`

> **Considered Best Practice: Avoid Captive Dependencies**
> A common mistake is injecting a deep, scoped service (for example, `MyDbContext`) into a singleton service. Because the singleton service lives forever, it will keep the scoped service in the container structure for the lifetime of the application, converting it to a singleton service. This can lead to memory leaks and erratic behavior across requests. ASP.NET Core throws an exception at runtime to help you detect this during development.

### Manual Scope Management with `IServiceScopeFactory`

In scenarios where you need to manually create and manage scopes (background workers, singleton services, or long running tasks), you can use `IServiceScopeFactory` to create scopes.

This would be particularly useful when properly controlling when a singleton service needs to use scoped dependencies without causing captive dependency problems.

Within continuously running services, you must not directly inject objects that require short lifespans, such as database connections. This code example solves this problem by creating a temporary workspace for each task using a "throw away" approach. The environment and necessary services are created when the process starts, and all are automatically cleaned up when the task ends. This method utilizes sources efficiently and prevents memory leaks.

```csharp
public class DataProcessingService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DataProcessingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessDataAsync()
    {
        // Create a new scope for this unit of work
        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var repository = scope.ServiceProvider.GetRequiredService<IDataRepository>();
            
            // Perform scoped work
            var data = await repository.GetPendingDataAsync();
            await dbContext.SaveChangesAsync();
        }
        // Scope is disposed here, releasing all scoped services
    }
}
```

**Key Points:**
- We should use `CreateAsyncScope()` when working with asynchronous disposal.
- It would be logical to use `CreateScope()` for synchronous scenarios.
- Always destroying scopes appropriately using `using` or `await using` statements is important for scope management and optimization.


Here is the detailed explanation, incorporating your text and adding the requested details for property injection.

## Constructor Injection vs. Property Injection

There are several ways a class can receive its dependencies. The two most common patterns are **constructor** and **property** injection.

### Constructor Injection

With constructor injection, a class retrieves its dependencies from the container via constructor parameters. With dependency injection (DI), the container will be responsible for creating instances of these dependencies and fetching them when the class is generated. This is one of the most common and recommended approaches to **ASP.NET Core Dependency Injection**.

```csharp
// Primary Constructors
public class OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    private readonly ILogger<OrderService> _logger = logger;
    public async Task<Order> GetOrderAsync(int orderId)
    {
        _logger.LogInformation("Fetching order {OrderId}", orderId);
        return await _orderRepository.GetByIdAsync(orderId);
    }
}

// Traditional Class Constructor
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Order> GetOrderAsync(int orderId)
    {
        _logger.LogInformation("Fetching order {OrderId}", orderId);
        return await _orderRepository.GetByIdAsync(orderId);
    }
}
```

#### Pros:

  * **Explicit Dependencies:** The constructor's signature explicitly states all **required** dependencies. A developer will immediately see what the class needs to function when calling it.
  * **Immutability:** Dependencies can be assigned to `readonly` fields so that they cannot be changed after the object is created. This will lead to more stable and predictable class behavior.
  * **Availability:** The class is guaranteed to have the required dependencies when created. It will not need to perform null checks on required services.
  * **Startup Validation:** If a required dependency is not registered in the DI container, the application will fail *on startup* (at runtime), making errors easy to detect early.

> **Best Practice: Avoid Asynchronous Operations in Constructors**
> A constructor is expected to be simple and fast. We should not perform asynchronous operations (`await`) or long running tasks within a constructor. This can lead to deadlocks and unpredictable application startup behavior. Using asynchronous factory patterns or `IHostedService` for asynchronous startup logic will fix this issue in most scenarios.


### Property Injection

With property injection (also known as "setter injection"), dependencies are provided through publicly settable properties on the class. The dependency is injected after the class is created.

This pattern is less common in ASP.NET Core because the built in DI container will not support it out of the box (other third party containers like Autofac or Ninject do support it).

Property injection is almost exclusively used for **optional dependencies**, which would be services that the class could use but doesn't need to perform its core functionality.

```csharp
public class ProductService
{
    private readonly IProductRepository _productRepository;

    // Injected via a public property
    public ILogger<ProductService>? Logger { get; set; }

    // Still injected via the constructor
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        // Must check if the optional dependency was injected before using it
        Logger?.LogInformation("Fetching product {ProductId}", productId);
        
        return await _productRepository.GetByIdAsync(productId);
    }
}
```

Since the built in container doesn't automatically set the `Logger` property, you would either have to use a different container or set it manually (which partially defeats the purpose of DI). This is why it's strongly discouraged for *required* dependencies.

#### Pros:

  * **Optional Dependencies:** Can be used to provide optional services. The class can function without the dependency, but if a dependency is provided, its behavior needs to be improved.
  * **Decoupling:** It can help to break up large classes or prevent over injection in the constructor (constructors with too many parameters), but this usually indicates that the class is doing too much (Single Responsibility Principle).

#### Cons:

  * **Hidden Dependencies:** It won't be immediately obvious from the constructor what the class might depend on. Because of the way it's implemented, this means a developer will need to examine the class's properties.
  * **Mutability:** This means that the dependency will not be readonly and can be changed at any time, which may lead to unforeseen situations and changes.
  * **Null Check:** The class should always check if the optional dependency is `null` before using it.
  * **No Container Support:** The default ASP.NET Core container will not inject properties. This makes this pattern unusable unless you use a different container or manually add dependencies.


## Registering Services in ASP.NET Core

Services are registered in the DI container in `Program.cs`. This means adding the services to the `IServiceCollection`.

### Basic and Factory based Registrations

You can add an interface to a concrete class or use a factory based registration style for complex initialization.

**In Program.cs**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Simple registration
builder.Services.AddScoped<ISimpleService, SimpleService>();

// Factory based registration
builder.Services.AddScoped<ISomeComplexService>(provider =>
{
    // Resolve other service
    var logger = provider.GetRequiredService<ILogger<SomeComplexService>>();
    var someValue = "CalculatedOrRetrievedValue";
    
    // Manually build with dependencies
    return new SomeComplexService(logger, someValue);
});
```

### Conditional `TryAdd` Registrations

When developing reusable libraries or building dependent applications, you may want to register a service only if another application has not already registered it, and you may want to check for it. The `TryAdd` method is used for this scenario.

**Available Methods:**
- `TryAddSingleton<TService, TImplementation>()` Adds the singleton if it is not already registered
- `TryAddScoped<TService, TImplementation>()` Adds scoped if not already registered.
- `TryAddTransient<TService, TImplementation>()` Adds a transient if not already registered.
- `TryAddEnumerable()` Adds to a service collection (for `IEnumerable<T>` resolution).

```csharp
builder.Services.AddSingleton<ILogger, CustomLogger>();

builder.Services.TryAddSingleton<ILogger, DefaultLogger>();

// CustomLogger is used because it was registered first
// TryAdd* only adds if the service type isn't already registered
```

### The Options Pattern (`IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`)

The **Options Pattern** is the recommended way to add configuration to your services. It provides type safe access to configuration sections and integrates with DI.

**Three different options:**

1. **`IOptions<T>`** Singleton, will be loaded once at startup.
2. **`IOptionsSnapshot<T>`** Scoped, will be reloaded per request. (useful for multi tenant scenarios.)
3. **`IOptionsMonitor<T>`** Individually triggered changes will be reloaded when the configuration changes.

```csharp
public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}
```

**In appsettings.json**
```json
{
  "ExternalApi": {
    "BaseUrl": "https://api.example.com",
    "ApiKey": "your-api-key",
    "TimeoutSeconds": 60
  }
}
```

**Inject and use in a service**
```csharp
public class ExternalApiClient
{
    private readonly ApiSettings _settings;
    private readonly ILogger<ExternalApiClient> _logger;
    private readonly HttpClient _httpClient;

    // Use IOptions<T> for singleton services
    public ExternalApiClient(
        IOptions<ApiSettings> options, 
        ILogger<ExternalApiClient> logger,
        HttpClient httpClient)
    {
        _settings = options.Value;
        _logger = logger;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _settings.ApiKey);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
    }

    public async Task<string> FetchDataAsync()
    {
        return await _httpClient.GetStringAsync("/data");
    }
}

public class DynamicConfigService
{
    private readonly IOptionsMonitor<ApiSettings> _optionsMonitor;

    public DynamicConfigService(IOptionsMonitor<ApiSettings> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
        
        _optionsMonitor.OnChange(settings =>
        {
            Console.WriteLine($"Configuration changed! New URL: {settings.BaseUrl}");
        });
    }

    public ApiSettings GetCurrentSettings() => _optionsMonitor.CurrentValue;
}
```

**In Program.cs**
```csharp

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ExternalApi"));
builder.Services.AddHttpClient<ExternalApiClient>();
```

**When to use each:**
- **`IOptions<T>`:** Used for settings that do not change during runtime.
- **`IOptionsSnapshot<T>`:** Used in scoped services where the configuration may differ per request.
- **`IOptionsMonitor<T>`:** Used when you need to react to configuration changes without restarting the application.

### Automating Registration with Assembly Scanning

In large projects, manually registering each service can be time consuming and error prone. While the built in container doesn't offer native assembly scanning, you can use reflection based utilities or third party libraries like **Scrutor** to automate service registration based on conventions.

**Example: Using Scrutor**

```csharp
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// It will scan the services according to the contract and automatically register them.
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()                    // Scan the current assembly
    .AddClasses(classes => classes.Where(type => 
        type.Name.EndsWith("Service")))           // Find all classes ending with "Service"
    .AsImplementedInterfaces()                    // Register them by their interfaces
    .WithScopedLifetime());                       // Use scoped lifetime

// More specific scanning
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(IRepository<>).Assembly)
    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());
```

**Example: Custom reflection based scanning (without external library).**

```csharp
namespace MyApp.Services;

using System.Reflection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // Find all classes implementing IService marker interface
        var serviceTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } 
                     && t.GetInterfaces().Any(i => i.Name == "IService"));

        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = serviceType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
            
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, serviceType);
            }
        }

        return services;
    }
}

// Usage in Program.cs
builder.Services.AddApplicationServices();
```

**Best Practices:**
- Using assembly scanning will be easier in large projects where many services are used in accordance with the rules.
- Your naming conventions should be clearly documented (for example, all classes ending in "Service" are automatically registered).
- In performance critical scenarios, the assembly scan should be performed carefully to avoid potential problems, as this may result in startup costs.
- For true reflection overhead in .NET 9, consider using source generators.


## Best Practices and Common Mistakes

### Design for Explicit Dependencies

This principle states that parent modules should not depend directly on lower level modules (such as services for data access, sending email, or specific API clients). Instead, both should depend on abstractions (interfaces).

This reverses the normal flow of dependencies, decoupling your code and making it more flexible and testable.

#### Example

##### Bad: Violates DIP (Tight Coupling)

Here, the top level `NotificationService` depends directly on the low level, concrete `EmailSender` class.

```csharp
// Low level service
public class EmailSender
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// High level service
public class NotificationService
{
    // A direct dependency on a CONCRETE class
    private readonly EmailSender _emailSender;

    public NotificationService()
    {
        // The top level class is responsible for creating its own dependencies.
        _emailSender = new EmailSender(); 
    }

    public void NotifyUser(string message)
    {
        _emailSender.SendEmail(message);
    }
}
```

**Problems:**

1.  **Difficult to Test:** You will not be able to test `NotificationService` without sending an email.
2. **Not Flexible:** But what if you want to send an SMS instead? You will need to modify the `NotificationService` class.

##### Good: Following DIP (Loose Coupling)

Here both classes depend on the `IMessageSender` interface.

```csharp
// The Abstraction (Interface)
public interface IMessageSender
{
    void Send(string message);
}

// Low level service (depends on the abstraction)
public class EmailSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending email: {message}");
    }
}

// Another low level service
public class SmsSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}

// High level service (also depends on the abstraction)
public class NotificationService
{
    // Dependency is on the Interface, not a concrete class
    private readonly IMessageSender _messageSender;

    // The dependency is injected via the constructor
    public NotificationService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void NotifyUser(string message)
    {
        _messageSender.Send(message);
    }
}
```

**Benefits:**

  * **Flexible:** `NotificationService` doesn't care whether it is `EmailSender` or `SmsSender`. The DI container can be configured to provide both because it is dependent on an interface.
  * **Testable:** You can create a `MockMessageSender` class that implements `IMessageSender` to use in your unit tests without sending a real message and perform your operations without needing any real information.

### Service Disposal and `IAsyncDisposable`

If your service contains disposable sources (such as network connections or file streams), it must implement `IDisposable` or `IAsyncDisposable`. The DI container will automatically call `Dispose` or `DisposeAsync` for you at the end of the service's lifetime. This is an important behavior to prevent source issues.

In .NET 9, asynchronous disposal is the preferred model for services that perform I/O operations during cleanup. The container manages both asynchronous and synchronous disposal operations in a controlled manner.

In this example, `MyNetworkService` gets `HttpClient` via DI (which it will not dispose of) but also creates its own `FileStream` resource, which it is responsible for disposing of asynchronously.

```csharp
public class MyNetworkService : IAsyncDisposable
{
    private readonly HttpClient _httpClient; 
    
    private readonly FileStream _logStream; 
    private bool _disposed = false;

    public MyNetworkService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _logStream = new FileStream($"log_{Guid.NewGuid()}.txt", 
            FileMode.CreateNew, FileAccess.Write, FileShare.None, 
            4096, useAsync: true);
    }

    public async Task<string> FetchDataAsync()
    {
        var data = await _httpClient.GetStringAsync("https://api.example.com/data");
        await _logStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(data));
        return data;
    }

    // The container calls this automatically when the scope ends
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        // Asynchronous cleanup for resources WE OWN
        // We do NOT dispose of _httpClient here.
        await _logStream.FlushAsync();
        await _logStream.DisposeAsync();
        
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
```

**Using `await using` for Manual disposal:**

When you manually create service instances outside of the DI container, you should use the `await using` syntax to ensure proper asynchronous disposal.

```csharp
// Assume HttpClient is coming from somewhere
public async Task ProcessDataAsync(HttpClient httpClient)
{
    await using var service = new MyNetworkService(httpClient);
    await service.FetchDataAsync();
    // DisposeAsync() is called automatically here
}
```

**Best Practices:**
- If your cleanup operation involves asynchronous operations (file I/O, database connections, network calls), it would be more appropriate to implement `IAsyncDisposable`.
- If your service can be used in both synchronous and asynchronous contexts, you can implement both `IDisposable` and `IAsyncDisposable`.
- The DI container will call `DisposeAsync()` if present; otherwise it will fallback to `Dispose()`.
- It is recommended to always call `GC.SuppressFinalize(this)` at the end of your disposal method to avoid unnecessary terminations.

### Handling Circular Dependencies
A circular dependency occurs when Service A depends on Service B, and Service B, in turn, depends on Service A. The ASP.NET Core container automatically detects this situation during object resolution (at parse time) and throws an `InvalidOperationException` to prevent a stack overflow, usually with a message detailing the dependency loop.

To resolve this, you must refactor your design to break the circular reference. The most common solution is to introduce a new intermediary abstraction (like an interface) that one of the services can depend on, breaking the direct loop.

**You Should Avoid Asynchronous Logic in Constructors**
- Constructors must be fast and synchronous. You should not perform `await` or long running operations.
- Asynchronous operation in constructors can lead to deadlocks and unpredictable initialization behavior.

For asynchronous initialization you should use `IHostedService`, factory patterns or lazy initialization.

```csharp
// Bad: Async work in constructor
public class BadService
{
    public BadService(IDataService dataService)
    {
        // This will deadlock or fail!
        var data = dataService.GetDataAsync().Result;
    }
}

// Good: Use IHostedService for async initialization
public class GoodInitializationService : IHostedService
{
    private readonly IDataService _dataService;

    public GoodInitializationService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Proper async initialization
        var data = await _dataService.GetDataAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

**Captive Dependency Issues**
- It is created by injecting a shorter lived service (Scoped) into a longer lived service (Singleton).
- The scoped service becomes "captive" and lives as long as the singleton service, which can lead to stale data and memory leaks.
- The container's **ValidateScopes** option will detect this at runtime.

```csharp
// Bad: Scoped service captured by singleton
builder.Services.AddSingleton<MySingletonService>(); // Holds DbContext forever!
builder.Services.AddScoped<AppDbContext>();

// Good: Use IServiceScopeFactory in singletons
public class MySingletonService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public MySingletonService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public async Task DoWorkAsync()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Use dbContext safely within this scope
    }
}
```

**Avoiding Static Shared State in Singleton Services**
- Singleton services should be stateless or have thread safe state management.
- Avoid using static fields or shared mutable states that can cause race conditions.

For immutable objects, the `ConcurrentDictionary` or appropriate locking mechanisms must be used.

```csharp
// Bad: Shared mutable state in singleton
public class BadCacheService
{
    private Dictionary<string, string> _cache = new(); // Not thread safe!

    public void Add(string key, string value) => _cache[key] = value;
}

// Good: Thread safe state management
public class GoodCacheService
{
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public void Add(string key, string value) => _cache[key] = value;
}
```

### Testing, Isolation, and Readability

One of the primary goals of DI is testability. In integration tests, you can use the WebApplicationFactory to override service registrations with mock applications.

```csharp
await using var application = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IExternalService, MockExternalService>();
        });
    });
```

## Performance Considerations

The performance of **Dependency injection** in ASP.NET Core is adequate for most applications, but it's worth being aware of the mechanics.

### Resolution Cost, Service Graph Caching, and Object Pooling

  * **Service Graph Caching:** When a service graph is first parsed, the container creates and caches an execution plan. This plan includes the entire dependency tree and how each object will be created. Subsequent solutions will use this cached plan, making them extremely fast.
  * **Transient and Singleton Resolving:** Transient services have a slightly higher creation cost because a new instance is created each time. However, this cost is generally negligible unless you are resolving thousands of transient services per request.
  * **Object Pool:** For high performance scenarios, it would be beneficial for performance management to consider using `ObjectPool<T>` from `Microsoft.Extensions.ObjectPool` to reuse expensive objects instead of creating new transient instances.

### Benchmarking DI Performance

Here is an example of a minimum benchmark comparing transient and singleton resolution times using `BenchmarkDotNet`.

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;

public class DependencyInjectionBenchmark
{
    private ServiceProvider _serviceProvider = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransientService, TransientService>();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddScoped<IScopedService, ScopedService>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Benchmark]
    public ITransientService ResolveTransient()
    {
        return _serviceProvider.GetRequiredService<ITransientService>();
    }

    [Benchmark]
    public ISingletonService ResolveSingleton()
    {
        return _serviceProvider.GetRequiredService<ISingletonService>();
    }

    [Benchmark]
    public IScopedService ResolveScoped()
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IScopedService>();
    }
}

public interface ITransientService { }
public class TransientService : ITransientService { }
public interface ISingletonService { }
public class SingletonService : ISingletonService { }
public interface IScopedService { }
public class ScopedService : IScopedService { }

// Results:
// | Method           | Mean      | Error    | StdDev   |
// |----------------- |----------:|---------:|---------:|
// | ResolveSingleton |  3.5 ns   | 0.02 ns  | 0.02 ns  | ← Fastest (cached instance)
// | ResolveTransient | 45.2 ns   | 0.31 ns  | 0.29 ns  | ← Allocation overhead
// | ResolveScoped    | 78.4 ns   | 0.52 ns  | 0.48 ns  | ← Scope creation + resolution
```

**Key Points:**
- Singleton resolution is almost instantaneous (cached instance invocation).
- Transient solution requires cost but is still fast.
- Scoped solution includes the overhead of creating scope.

For most applications, these differences are not noticeable. It should only be optimized if profiling reveals that DI is a bottleneck.


### Startup Time and Compile Time DI

Application startup time is the primary driver of performance. .NET uses **Source Generated Dependency Injection** to move dependency graph resolution from runtime to compile time.

**Benefits:**
  - **Faster Startup:** There is no runtime reflection to create the service graph.
  - **Reduced Memory Usage:** It produces smaller runtime.
  - **AOT Friendly:** Provides support for Native AOT compilation, which is critical for cloud native and containerized applications.
  - **Compile Time Validation:** Allows catching missing service records at compile time rather than runtime.

**How to Enable**

**1. For Core Dependency Injection (DI) Generation**

This is used as the main constructor that creates the optimized service provider for `AddScoped`, `AddSingleton`, etc.

  * **Activation** This will be automatically enabled when you publish with native AOT.
  * **Project file**
    ```xml
    <PropertyGroup>
        <PublishAot>true</PublishAot>
    </PropertyGroup>
    ```

**2. For Configuration Binding Generation**

This is used to bind settings from sources like `appsettings.json` to your C# classes (`Bind`, `Configure`).

  * **Project file:**
    ```xml
    <PropertyGroup>
        <EnableConfigurationBindingGenerator>true</EnableConfigurationBindingGenerator>
    </PropertyGroup>
    ```

**Example Code (using all generators):**

```csharp
var builder = WebApplication.CreateBuilder(args);

// This call is optimized by 'EnableConfigurationBindingGenerator'
builder.Services.Configure<MyOptions>(builder.Configuration.GetSection("MyOptions"));

// These calls are optimized by the Core DI generator (when PublishAot=true)
builder.Services.AddScoped<IMyService, MyService>();
var app = builder.Build();
```

When all generators are enabled, the compiler generates optimized service registration and parsing code, eliminating the reflection overhead.

**When to Use:**

  - Microservices and serverless functions (when it is desired to minimize cold start time).
  - Native AOT scenarios (e.g. containerized applications, edge computing).
  - Large applications with complex dependency graphs.

## Advanced Dependency Injection Patterns in .NET

### Keyed Services

Keyed services, introduced in .NET 8, allow you to register multiple implementations of an interface and resolve a specific implementation using a key. This will be a useful feature for polymorphic scenarios where you need to choose a strategy at runtime.

```csharp
// Registration
builder.Services.AddKeyedSingleton<INotificationService, EmailNotificationService>("email");
builder.Services.AddKeyedSingleton<INotificationService, SmsNotificationService>("sms");

// Resolution in a consumer class
public class NotificationController([FromKeyedServices("email")] INotificationService emailService)
{
    // ...
}
```

### Open Generic Registrations

To avoid manually registering each generic implementation (e.g., `IRepository<User>`, `IRepository<Product>`), you can use an explicit global registration.

```csharp
// This single line registers the Repository<T> for any T requested via IRepository<T>
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

### The Decorator Pattern

Decorators allow you to add functionality to a service without modifying it. This is a perfect example of the Open/Closed Principle. You can register a decorator that wraps the original service and adds cross cutting functionality like logging, caching, or validation.

**Without third party libraries (manual approach):**

```csharp
// Original service interface
public interface IOrderProcessor
{
    Task ProcessOrderAsync(Order order);
}

// Base implementation
public class OrderProcessor : IOrderProcessor
{
    public async Task ProcessOrderAsync(Order order)
    {
        await Task.Delay(100);
        Console.WriteLine($"Order {order.Id} processed.");
    }
}

// Logging decorator
public class LoggingOrderProcessorDecorator : IOrderProcessor
{
    private readonly IOrderProcessor _inner;
    private readonly ILogger<LoggingOrderProcessorDecorator> _logger;

    public LoggingOrderProcessorDecorator(IOrderProcessor inner, ILogger<LoggingOrderProcessorDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        _logger.LogInformation("Processing order {OrderId}...", order.Id);
        try
        {
            await _inner.ProcessOrderAsync(order);
            _logger.LogInformation("Order {OrderId} processed successfully.", order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process order {OrderId}.", order.Id);
            throw;
        }
    }
}

// Manual registration (layering decorators)
builder.Services.AddScoped<OrderProcessor>();
builder.Services.AddScoped<IOrderProcessor>(provider =>
{
    var baseProcessor = provider.GetRequiredService<OrderProcessor>();
    var logger = provider.GetRequiredService<ILogger<LoggingOrderProcessorDecorator>>();
    return new LoggingOrderProcessorDecorator(baseProcessor, logger);
});
```

**With Scrutor library (recommended for complex scenarios):**

```csharp
builder.Services.AddScoped<IOrderProcessor, OrderProcessor>();
builder.Services.Decorate<IOrderProcessor, LoggingOrderProcessorDecorator>();
// Add more decorators
builder.Services.Decorate<IOrderProcessor, CachingOrderProcessorDecorator>();
```

**Middleware Integration Context**

Decorators work in a similar way with ASP.NET Core middleware. While the middleware operates at the HTTP pipeline level, decorators operate at the service level, allowing you to apply cross cutting concerns to business logic independent of HTTP concerns.

### Conditional Registrations

You can conditionally register services based on runtime configuration or environment.

```csharp
var builder = WebApplication.CreateBuilder(args);

// You can register different applications depending on the environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, FakeEmailService>();
}
else
{
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
}

// Register based on configuration
var useRedis = builder.Configuration.GetValue<bool>("UseRedisCache");
if (useRedis)
{
    builder.Services.AddStackExchangeRedisCache(options => { /* ... */ });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}
```

### Child Containers and Nested Scopes

While the built in ASP.NET Core container doesn't support "subcontainers" like Autofac or other third party containers, you can achieve similar isolation using scopes.

It's important to differentiate this from the Service Locator anti pattern, which involves directly injecting IServiceProvider to manually resolve dependencies. Instead, the correct approach (especially within Singleton services) is to inject `IServiceScopeFactory`.

**Comprehensive embedded approach**
```csharp
public class ParentService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ParentService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task DoIsolatedWorkAsync()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        
        // Resolve services from the new scope's provider
        var isolatedService = scope.ServiceProvider.GetRequiredService<IIsolatedService>();
        await isolatedService.DoWorkAsync();
        // All services resolved from 'scope.ServiceProvider' are disposed here
    }
}
```

**Third party containers (Autofac example)**
```csharp
// Autofac supports real subcontainers with invalid records
var childLifetimeScope = container.BeginLifetimeScope(builder =>
{
    builder.RegisterType<MockDatabaseService>().As<IDatabaseService>();
});
```

The built in container's scoping mechanism is simpler but sufficient for most scenarios. You can use third party containers only when you need advanced features like property injection, assembly scanning with rules, or complex lifetime management.

### Testing with DI and `ConfigureTestServices`

One of DI's greatest strengths is testability. In integration tests, you can replace real services with mock or simulated services using WebApplicationFactory and ConfigureTestServices.

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ProcessOrder_ReturnsSuccess()
    {
        //Replace real service with mock
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                //Remove actual service
                services.RemoveAll<IOrderProcessor>();
                
                // Add mock service
                services.AddScoped<IOrderProcessor, MockOrderProcessor>();
                
                // Or use a mocking framework
                var mockProcessor = new Mock<IOrderProcessor>();
                mockProcessor.Setup(x => x.ProcessOrderAsync(It.IsAny<Order>()))
                            .ReturnsAsync(true);
                services.AddScoped(_ => mockProcessor.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/orders", new Order { Id = 1 });

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

**Key Benefits:**
- You can replace expensive external dependencies with in memory mocks.
- You can test business logic in isolation.
- You can run fast and accurate tests without needing external dependencies.

## Example: A Background Service

A common scenario where DI lifecycle management is critical is with singletons, such as background workers or IHostedServices. You cannot directly add a scoped service like DbContext to this service. Instead, you'd be better off adding an IServiceScopeFactory to manually create scopes.

**Hosted Service (`OrderProcessorWorker.cs`):**

```csharp
public class OrderProcessorWorker : BackgroundService
{
    private readonly ILogger<OrderProcessorWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    // Inject IServiceScopeFactory, not DbContext
    public OrderProcessorWorker(ILogger<OrderProcessorWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing new orders...");

            // Create a new scope for this unit of work
            await using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var newOrders = await orderRepository.GetNewOrdersAsync();
                
                foreach (var order in newOrders)
                {
                    // Process order...
                }
                
                await orderRepository.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
```

**Registration (`Program.cs`):**

```csharp
builder.Services.AddDbContext<AppDbContext>(/* ... */);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddHostedService<OrderProcessorWorker>();
```

This pattern ensures that each run of the worker uses a new `DbContext`, preventing problems such as memory leaks or stale data.

> While this example uses a simple `Task.Delay` loop within the `BackgroundService`, a robust pattern for managing decoupled background tasks involves an in memory queue. You can learn how to build this system by following this guide: [How to Build an In Memory Background Job Queue in ASP.NET Core From Scratch](https://abp.io/community/articles/how-to-build-an-in-memory-background-job-queue-in-asp.net-core-from-scratch-pai2zmtr).

## Conclusion

Understanding the **ASP.NET Core Dependency Injection** framework is essential for any .NET developer. By understanding the built in IoC container, choosing the right service lifecycles, and opting for explicit constructor injection, you can create modular, testable, and maintainable applications.

**.NET brings significant enhancements to the DI ecosystem**

- **Source Generated DI** for faster startup and Native AOT support
- **Keyed Services** for advanced polymorphic resolution scenarios
- **Enhanced lifetime validation** catches captive dependencies at development time
- **Improved `IAsyncDisposable`** support for proper source cleanup*
- **Good integration with C# features** such as primary constructors

By embracing these features and implementing patterns such as decorators, manual scoping with `IServiceScopeFactory`, the Option Pattern for configuration, and proper asynchronous disposal, you can solve complex architectural challenges cleanly and efficiently.

**Key Points**

- **Always inject dependencies via constructors** explicit, immutable, testable
- **Understanding lifetime implications** avoid dependent dependencies, you can use `IServiceScopeFactory` on singletons
- **Leverage the Option Pattern** type safe, validated configuration injection
- **You can use TryAdd methods** create mergeable records
- **Leverage DI in your tests** you can use `ConfigureTestServices` to inject mocks
- **Measure performance first; don't assume DI is a bottleneck** The internal container is efficient. Use a profiler to find real slowdowns before trying to optimize DI.
- **Consider source generation** for microservices, serverless and AOT scenarios

The transition from legacy, fragmented DI environments to a unified, performant, and compile time optimized dependency injection system represents a significant development in the platform's history. Understanding and leveraging these capabilities is crucial for building high performance, cloud native .NET applications.

### Further Reading

- [ABP Dependency Injection](https://abp.io/docs/10.0/framework/fundamentals/dependency-injection)
- [Official Microsoft Docs on Dependency Injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Source Generators for Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
- [IHttpClientFactory with .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)
- [Keyed Services DI Container](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-9.0#keyed-services)
- [Use Scoped Services Within a Scoped Service](https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service)
- [Scrutor](https://github.com/khellang/Scrutor)
