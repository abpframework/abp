```json
//[doc-seo]
{
    "Description": "Learn how the ABP Framework uses Correlation IDs to trace and correlate operations across HTTP requests, distributed events, audit logs, and microservices."
}
```

# Correlation ID

A **Correlation ID** is a unique identifier that is assigned to a request or operation and propagated across all related processing steps. It allows you to **trace** and **correlate** logs, events, and operations that belong to the same logical transaction, even when they span multiple services or components.

ABP provides a built-in correlation ID infrastructure that:

- **Automatically assigns** a unique correlation ID to each incoming HTTP request (or uses the one provided by the caller).
- **Propagates** the correlation ID through distributed event bus messages, HTTP client calls, audit logs, security logs, and Serilog log entries.
- **Provides a simple API** (`ICorrelationIdProvider`) to get or change the current correlation ID in your application code.

## `AbpCorrelationIdMiddleware`

`AbpCorrelationIdMiddleware` is an ASP.NET Core middleware that handles correlation ID management for HTTP requests. It is automatically added to the request pipeline when you use ABP's application builder.

The middleware performs the following steps for each incoming HTTP request:

1. **Reads** the correlation ID from the incoming request's `X-Correlation-Id` header (configurable via `AbpCorrelationIdOptions` as explained below).
2. **Generates** a new correlation ID (`Guid.NewGuid().ToString("N")`) if the request does not contain one.
3. **Sets** the correlation ID in the current async context using `ICorrelationIdProvider`, making it available throughout the request pipeline.
4. **Writes** the correlation ID to the response header (if `SetResponseHeader` option is enabled).

You can add the middleware to your request pipeline by calling the `UseCorrelationId` extension method:

```csharp
app.UseCorrelationId();
```

> This is already configured in the application startup template. You typically don't need to add it manually.

## `ICorrelationIdProvider`

`ICorrelationIdProvider` is the core service for working with correlation IDs. It allows you to retrieve the current correlation ID or temporarily change it.

```csharp
public interface ICorrelationIdProvider
{
    string? Get();

    IDisposable Change(string? correlationId);
}
```

- `Get()`: Returns the current correlation ID for the executing context. Returns `null` if no correlation ID has been set.
- `Change(string? correlationId)`: Changes the correlation ID for the current context and returns an `IDisposable` object. When the returned object is disposed, the correlation ID is restored to its previous value.

### Using `ICorrelationIdProvider`

You can inject `ICorrelationIdProvider` into any service to access the current correlation ID:

```csharp
public class MyService : ITransientDependency
{
    public ILogger<MyService> Logger { get; set; }

    private readonly ICorrelationIdProvider _correlationIdProvider;

    public MyService(ICorrelationIdProvider correlationIdProvider)
    {
        Logger = NullLogger<MyService>.Instance;
        _correlationIdProvider = correlationIdProvider;
    }

    public async Task DoSomethingAsync()
    {
        // Get the current correlation ID
        var correlationId = _correlationIdProvider.Get();

        // Use it for logging, tracing, etc.
        Logger.LogInformation("Processing with Correlation ID: {CorrelationId}", correlationId);

        await SomeOperationAsync();
    }
}
```

### Changing the Correlation ID

You can temporarily change the correlation ID using the `Change` method. This is useful when you want to create a new scope with a different correlation ID:

```csharp
public async Task ProcessAsync()
{
    var currentCorrelationId = _correlationIdProvider.Get();
    // currentCorrelationId = "abc123"

    using (_correlationIdProvider.Change("new-correlation-id"))
    {
        var innerCorrelationId = _correlationIdProvider.Get();
        // innerCorrelationId = "new-correlation-id"
    }

    var restoredCorrelationId = _correlationIdProvider.Get();
    // restoredCorrelationId = "abc123" (restored to original)
}
```

The `Change` method returns an `IDisposable`. When disposed, the correlation ID is automatically restored to its previous value. This pattern supports nested scopes safely.

### Default Implementation

The default implementation (`DefaultCorrelationIdProvider`) uses `AsyncLocal<string?>` to store the correlation ID. This ensures that the correlation ID is isolated per async execution flow and is thread-safe.

## `AbpCorrelationIdOptions`

You can configure the correlation ID behavior using `AbpCorrelationIdOptions`:

```csharp
Configure<AbpCorrelationIdOptions>(options =>
{
    options.HttpHeaderName = "X-Correlation-Id";
    options.SetResponseHeader = true;
});
```

- `HttpHeaderName` (default: `"X-Correlation-Id"`): The HTTP header name used to read/write the correlation ID. You can change this if your infrastructure uses a different header name.
- `SetResponseHeader` (default: `true`): If `true`, the middleware automatically adds the correlation ID to the HTTP response headers. Set it to `false` if you don't want to expose the correlation ID in response headers.

## Correlation ID Across ABP Services

One of the most valuable aspects of ABP's correlation ID infrastructure is that it **automatically propagates** the correlation ID across various system boundaries. This allows you to trace a single user action as it flows through multiple services and components.

### HTTP Client Calls

When you use ABP's [dynamic client proxies](../api-development/dynamic-csharp-clients.md) to call remote services, the correlation ID is automatically added to the outgoing HTTP request headers. This means downstream services will receive the same correlation ID, enabling end-to-end tracing across microservices.

```
Client Request (X-Correlation-Id: abc123)
  → Service A (receives abc123, sets in context)
    → Service B via HTTP Client Proxy (forwards abc123 in header)
      → Service C via HTTP Client Proxy (forwards abc123 in header)
```

No manual configuration is required. ABP's `ClientProxyBase` automatically reads the current correlation ID from `ICorrelationIdProvider` and adds it as a request header.

### Distributed Event Bus

When you publish a [distributed event](./event-bus/distributed/index.md), ABP automatically attaches the current correlation ID to the outgoing event message. When the event is consumed (potentially by a different service), the correlation ID is extracted from the message and set in the consumer's context.

> This works with all supported event bus providers, including [RabbitMQ](./event-bus/distributed/rabbitmq.md), [Kafka](./event-bus/distributed/kafka.md), [Azure Service Bus](./event-bus/distributed/azure.md) and [Rebus](./event-bus/distributed/rebus.md).

```
Service A publishes event (CorrelationId: abc123)
  → Event Bus (carries abc123 in message metadata)
    → Service B receives event (CorrelationId restored to abc123)
```

### Audit Logging

ABP's [audit logging](./audit-logging.md) system automatically captures the current correlation ID when creating audit log entries. This is stored in the `CorrelationId` property of `AuditLogInfo`, allowing you to query and filter audit logs by correlation ID.

This is particularly useful for:

- Tracing all database changes made during a single request.
- Correlating audit log entries across multiple services for the same user action.
- Debugging and investigating issues by filtering logs with a specific correlation ID.

### Security Logging

Similar to audit logging, ABP's security log system also captures the current correlation ID. When security-related events are logged (such as login attempts, permission checks, etc.), the correlation ID is included in the `SecurityLogInfo.CorrelationId` property.

### Serilog Integration

If you use the **ABP Serilog integration**, the correlation ID is automatically added to the Serilog log context as a property. This means every log entry within a request will include the correlation ID, making it easy to filter and search logs.

The correlation ID is enriched as a log property named `CorrelationId` by default. You can use it in your Serilog output template or structured log queries.

## See Also

- [Audit Logging](./audit-logging.md)
- [Distributed Event Bus](./event-bus/distributed/index.md)
- [Dynamic Client Proxies](../api-development/dynamic-csharp-clients.md)
